#!/usr/bin/env python3
import socket
import time

import cv2
import numpy as np
import rclpy
from rclpy.node import Node
from rclpy.qos import qos_profile_sensor_data
from std_srvs.srv import SetBool
from sensor_msgs.msg import Image
from cv_bridge import CvBridge


class CyberDogCameraUdpSender(Node):
    def __init__(self):
        super().__init__('cyberdog_camera_udp_sender', namespace='/mi1036358')

        # =========================
        # CONFIG GENERAL
        # =========================
        self.quest_ip = '192.168.242.18'   # Cambiar si la IP de Quest cambia
        self.quest_port = 5006

        self.output_width = 320
        self.output_height = 240
        self.jpeg_quality = 55

        # =========================
        # TOPICO ACTIVO
        # =========================
        self.active_topic = '/mi1036358/camera/color/image_raw'
        self.active_mode = 'COLOR'
        # self.active_topic = '/mi1036358/camera/aligned_depth_to_color/image_raw'
        # self.active_mode = 'DEPTH'
        

        # =========================
        # TOPICO DEPTH (PREPARADO)
        # =========================
        # Si en algún momento quieres usar profundidad, puedes cambiar:
        # self.active_topic = '/mi1036358/camera/aligned_depth_to_color/image_raw'
        # self.active_mode = 'DEPTH'
        #
        # También podrías usar depth puro:
        # self.active_topic = '/mi1036358/camera/depth/image_rect_raw'
        # self.active_mode = 'DEPTH'

        self.bridge = CvBridge()
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

        self.frames_sent = 0
        self.last_log_time = time.time()
        self.last_frame_time = None
        self.current_fps = 0.0

        # Activar cámara
        self.enable_camera()

        # Suscripción
        self.sub = self.create_subscription(
            Image,
            self.active_topic,
            self.image_callback,
            qos_profile_sensor_data
        )

        self.get_logger().info(f'Modo activo: {self.active_mode}')
        self.get_logger().info(f'Tópico activo: {self.active_topic}')
        self.get_logger().info(f'Enviando UDP a {self.quest_ip}:{self.quest_port}')
        self.get_logger().info(
            f'Resolución de salida: {self.output_width}x{self.output_height} | JPEG quality={self.jpeg_quality}'
        )

    def enable_camera(self):
        client = self.create_client(SetBool, '/mi1036358/camera/enable')
        if not client.wait_for_service(timeout_sec=5.0):
            self.get_logger().warning('Servicio /mi1036358/camera/enable no disponible')
            return

        req = SetBool.Request()
        req.data = True
        future = client.call_async(req)
        rclpy.spin_until_future_complete(self, future, timeout_sec=5.0)

        if future.result() is not None:
            self.get_logger().info(f'Cámara activada: success={future.result().success}')
        else:
            self.get_logger().warning('No hubo respuesta al activar la cámara')

    def image_callback(self, msg: Image):
        try:
            # Medición simple de frecuencia real del callback
            now = time.time()
            if self.last_frame_time is not None:
                dt = now - self.last_frame_time
                if dt > 0:
                    self.current_fps = 1.0 / dt
            self.last_frame_time = now

            # =========================
            # PROCESAMIENTO SEGÚN MODO
            # =========================
            if self.active_mode == 'COLOR':
                # Igual que en el script que se veía fluido
                if msg.encoding.lower() == 'rgb8':
                    frame = self.bridge.imgmsg_to_cv2(msg, 'rgb8')
                    frame = cv2.cvtColor(frame, cv2.COLOR_RGB2BGR)
                else:
                    frame = self.bridge.imgmsg_to_cv2(msg, 'bgr8')

            elif self.active_mode == 'DEPTH':
                frame = self.bridge.imgmsg_to_cv2(msg, 'passthrough')

                # Normalización para visualización
                if frame.dtype == np.uint16:
                    frame = cv2.normalize(frame, None, 0, 255, cv2.NORM_MINMAX).astype(np.uint8)

                if len(frame.shape) == 2:
                    frame = cv2.applyColorMap(frame, cv2.COLORMAP_JET)

            else:
                self.get_logger().warning(f'Modo no soportado: {self.active_mode}')
                return

            # Resize para transmisión
            frame = cv2.resize(frame, (self.output_width, self.output_height))

            # JPEG
            ok, encoded = cv2.imencode(
                '.jpg',
                frame,
                [int(cv2.IMWRITE_JPEG_QUALITY), self.jpeg_quality]
            )

            if not ok:
                self.get_logger().warning('No se pudo comprimir frame JPEG')
                return

            jpeg_bytes = encoded.tobytes()

            # Envío UDP
            self.sock.sendto(jpeg_bytes, (self.quest_ip, self.quest_port))

            self.frames_sent += 1

            # Logs cada 2 s
            log_now = time.time()
            if log_now - self.last_log_time >= 2.0:
                elapsed = log_now - self.last_log_time
                fps_sent = self.frames_sent / elapsed if elapsed > 0 else 0.0

                self.get_logger().info(
                    f'Modo={self.active_mode} | callback_fps~{self.current_fps:.2f} | '
                    f'fps_enviados~{fps_sent:.2f} | bytes={len(jpeg_bytes)}'
                )

                self.frames_sent = 0
                self.last_log_time = log_now

        except Exception as e:
            self.get_logger().error(f'Error procesando/enviando frame: {e}')


def main():
    rclpy.init()
    node = CyberDogCameraUdpSender()
    try:
        rclpy.spin(node)
    finally:
        node.destroy_node()
        rclpy.shutdown()


if __name__ == '__main__':
    main()
