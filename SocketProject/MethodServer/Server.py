from socket import *
import threading
import time
from MethodServer.StrategyPatternModel import Context


# 处理数据的方法
def handle(bytes_data):
    require_name = bytes_data.split(SPLIT_STR.encode('utf-8'))[0].decode('utf-8')
    solve = context.invoke_mclass(require_name, bytes_data, SPLIT_STR)
    return solve

# 将方法类返回的结果处理成可供发送的文本形式
def format_solve(solve):
    solve_str = ''
    if isinstance(solve, tuple) or isinstance(solve, list):
        for para in solve:
            solve_str += str(para) + SPLIT_STR
    else:
        solve_str = str(solve)
    byte_solve = solve_str.encode('utf-8') + TRAILER
    return byte_solve


def receive_thread_work(the_socket, addr):
    remain = b''
    while True:
        this_time_data, remain = trailer_receive(the_socket, remain)
        print(len(this_time_data))
        if this_time_data == b'' and remain == b'':
            break
        if isinstance(this_time_data, bytes) and this_time_data != b'':
            try:
                # 这里基本以文本形式发送，没有考虑二进制形式程序间的通信
                solve = handle(this_time_data)
                byte_solve = format_solve(solve)
                the_socket.send(byte_solve)
            except Exception as e:
                print('Error occur in handle ' + str(addr) + '! ' + 'Error message: ' + repr(e))
                break
        elif isinstance(this_time_data, str):  # 肯定是错误信息
            print('Error occur in receive ' + str(addr) + '! ' + this_time_data)
            break

    try:
        the_socket.close()
    finally:
        print(addr, ' end')


# 尾标法接收数据
# 这个算法开始好像不能处理 客户端发送的文本正好是TRAILER的问题，之后解决了
def trailer_receive(the_socket, remain):
    total_data = []
    try:
        # 尝试接收数据之前，先处理上一次剩下的一段数据
        #                                                           防止输出Trailer
        if isinstance(remain, bytes) and remain != b'' and remain != TRAILER:
            if TRAILER in remain:
                trailer_start_pos = remain.find(TRAILER)
                total_data.append(remain[:trailer_start_pos])
                remain = remain[trailer_start_pos + len(TRAILER):]
                return b''.join(total_data), remain
            else:
                # 接上上次剩下的数据，开始处理
                total_data.append(remain)
        begin_time = time.time()
        while True:
            buffer = the_socket.recv(BUFSIZE)

            # 规定时间内只有空数据的话，则放弃
            if buffer:
                begin_time = time.time()
            else:
                if time.time() - begin_time > MAX_BLOCK_TIME:
                    break

            # 开始处理缓冲区数据
            if TRAILER in buffer:
                trailer_start_pos = buffer.find(TRAILER)
                total_data.append(buffer[:trailer_start_pos])
                remain = buffer[trailer_start_pos + len(TRAILER):]
                break
            total_data.append(buffer)
            if len(total_data) > 1:
                # check if end_of_data was split
                # 这个检查方式决定了BUFSIZE > len(TRAILER) / 2
                last_pair = total_data[-2] + total_data[-1]
                if TRAILER in last_pair:
                    trailer_start_pos = last_pair.find(TRAILER)
                    total_data[-2] = last_pair[:trailer_start_pos]
                    remain = last_pair[trailer_start_pos + len(TRAILER):]
                    total_data.pop()
                    break
    except Exception as e:
        return 'Error message: ' + repr(e) + ' Last error total data: ' + b''.join(total_data).decode('utf-8'), remain
    return b''.join(total_data), remain


def start(*method_class_tuple):
    tcp_server_socket = socket(AF_INET, SOCK_STREAM)
    tcp_server_socket.bind(ADDR)  # 服务端要与Socket绑定
    tcp_server_socket.listen(MAXLISTEN)

    # 添加进外界规定好的方法
    context.append_mclass(method_class_tuple)

    while True:
        try:
            print('Waiting for connection')
            tcp_client_socket, address = tcp_server_socket.accept()
            print('Connect from:', address)

            #开启一个子线程，使用方法来接收数据
            t = threading.Thread(target=receive_thread_work, args=(tcp_client_socket, address))
            t.start()
        except:
            tcp_server_socket.shutdown()
            tcp_server_socket.close()


HOST = ''  # host空白表示可以用任何可用的地址
PORT = 10022
BUFSIZE = 1024
ADDR = (HOST, PORT)
MAXLISTEN = 20
SPLIT_STR = 'the split str'
# BUFSIZE不能比len(TRAILER) / 2还要小
TRAILER = 'something useable as an end marker'.encode('utf-8')
MAX_BLOCK_TIME = 2  # 秒
# 这是服务器端维护方法的上下文
context = Context()