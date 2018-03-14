
# TCP接收数据的方式：尾标法接收数据
def trailer_receive(the_socket, remain):
    total_data = []
    try:
        if remain is bytes and remain != b'':
            total_data.append(remain)
        while True:
            buffer = the_socket.recv(BUFSIZE)
            if not buffer:
                break
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
        return 'Error message: '+repr(e)+' Last error total data: '+b''.join(total_data).decode('utf-8'),remain
    return b''.join(total_data), remain
