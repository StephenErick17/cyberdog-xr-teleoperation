#!/usr/bin/env python3
import socket
import threading

import rclpy
from rclpy.node import Node
from motion_msgs.msg import SE3VelocityCMD, ActionRequest


class UDPToROSBridge(Node):
    def __init__(self):
        super().__init__('udp_to_ros_bridge', namespace='/mi1036358')

        # UDP
        self.udp_ip = '0.0.0.0'
        self.udp_port = 5005

        # ROS publishers directos
        self.vel_pub = self.create_publisher(SE3VelocityCMD, 'body_cmd', 10)
        self.action_pub = self.create_publisher(ActionRequest, 'cyberdog_action', 10)

        # Action request ID
        self.request_id = 100

        # Socket UDP
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.sock.bind((self.udp_ip, self.udp_port))

        self.get_logger().info(f'Bridge UDP escuchando en {self.udp_ip}:{self.udp_port}')
        self.get_logger().info('Soporta mensajes:')
        self.get_logger().info('  cmd:lx,ly,az')
        self.get_logger().info('  action:x')

        self.listen_thread = threading.Thread(target=self.listen_udp, daemon=True)
        self.listen_thread.start()

    def listen_udp(self):
        while rclpy.ok():
            try:
                data, addr = self.sock.recvfrom(1024)
                message = data.decode('utf-8').strip()
                self.get_logger().info(f'Mensaje UDP recibido: "{message}" desde {addr}')
                self.process_message(message)
            except Exception as e:
                self.get_logger().error(f'Error recibiendo UDP: {e}')

    def process_message(self, message: str):
        try:
            # ===== Movimiento continuo =====
            if message.startswith('cmd:'):
                payload = message[4:]
                parts = payload.split(',')

                if len(parts) != 3:
                    self.get_logger().warning(f'Formato cmd inválido: {message}')
                    return

                lx = float(parts[0].strip())
                ly = float(parts[1].strip())
                az = float(parts[2].strip())

                cmd = self.create_velocity_command(lx, ly, az)
                self.vel_pub.publish(cmd)

                self.get_logger().info(
                    f'Publicado en /mi1036358/body_cmd: lx={lx:.3f}, ly={ly:.3f}, az={az:.3f}'
                )
                return

            # ===== Acciones discretas =====
            if message.startswith('action:'):
                payload = message[7:]
                action_code = int(payload.strip())

                action_map = {
                    1: 3,    # StandUp -> gait 3
                    2: 2,    # LieDown -> gait 2
                    3: 10,   # Sit -> gait 10
                    4: 7,    # SlowGait -> gait 7
                    5: 8,    # NormalGait -> gait 8
                    6: 11,   # Jump -> gait 11
                }

                if action_code not in action_map:
                    self.get_logger().warning(f'Action code desconocido: {action_code}')
                    return

                gait_value = action_map[action_code]
                self.send_action(gait_value)
                return

            self.get_logger().warning(f'Mensaje no reconocido: {message}')

        except ValueError:
            self.get_logger().warning(f'No se pudo convertir el valor en: {message}')
        except Exception as e:
            self.get_logger().error(f'Error procesando mensaje "{message}": {e}')

    def send_action(self, gait_value: int):
        msg = ActionRequest()
        msg.type = 2
        msg.request_id = self.request_id
        self.request_id += 1

        msg.gait.timestamp.sec = 0
        msg.gait.timestamp.nanosec = 0
        msg.gait.gait = int(gait_value)

        msg.mode.timestamp.sec = 0
        msg.mode.timestamp.nanosec = 0
        msg.mode.control_mode = 0
        msg.mode.mode_type = 0

        msg.order.timestamp.sec = 0
        msg.order.timestamp.nanosec = 0
        msg.order.id = 0
        msg.order.para = 0.0

        msg.timeout = 35
        self.action_pub.publish(msg)
        self.get_logger().info(f'Action gait enviado: {gait_value} (request_id={msg.request_id})')

    def create_velocity_command(self, lx=0.0, ly=0.0, az=0.0):
        cmd = SE3VelocityCMD()
        cmd.sourceid = 2
        cmd.velocity.frameid.id = 1

        now = self.get_clock().now().nanoseconds
        cmd.velocity.timestamp.sec = int(now // 1_000_000_000)
        cmd.velocity.timestamp.nanosec = int(now % 1_000_000_000)

        cmd.velocity.linear_x = float(lx)
        cmd.velocity.linear_y = float(ly)
        cmd.velocity.linear_z = 0.0
        cmd.velocity.angular_x = 0.0
        cmd.velocity.angular_y = 0.0
        cmd.velocity.angular_z = float(az)
        return cmd


def main(args=None):
    rclpy.init(args=args)
    node = UDPToROSBridge()

    try:
        rclpy.spin(node)
    except KeyboardInterrupt:
        pass
    finally:
        node.get_logger().info('Cerrando bridge UDP → ROS 2')
        node.destroy_node()
        rclpy.shutdown()


if __name__ == '__main__':
    main()
