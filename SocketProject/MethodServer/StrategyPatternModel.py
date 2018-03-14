from abc import ABCMeta, abstractmethod
import re  # 这里涉及到由发送过来的字符串匹配已知的字符串


class IMethod(metaclass=ABCMeta):

    # 基于字符串的方法接口
    # 从list_str 中第二个开始，需要使用者根据具体情况自行定义list_str中的文本如何转换
    # 并处理然后返回数据
    @abstractmethod
    def invoke_algorithm(self, str_list):
        pass


class IBinaryFileMethod(metaclass=ABCMeta):

    # 继承这个接口的算法接收来自服务器传来的byte算法
    @abstractmethod
    def invoke_algorithm(self, bynary_list):
        pass


# 维护一个含有IMethod方法类的的数组
# 负责接收‘方法名（参数1，参数2...）’的文本

class Context():

    # 外界——服务器脚本加入方法需要的方法类
    def __init__(self):
        self.method_class_list = []

    # 将元祖传入上下文，构成一个含有很多方法类的数组
    # 这种方法好像不能有重名
    def append_mclass(self, method_class_tuple):
        if method_class_tuple:
            for mclass in method_class_tuple:
                if isinstance(mclass, IMethod) or isinstance(mclass, IBinaryFileMethod):
                    self.method_class_list.append(mclass)

    def invoke_mclass(self, require_name, binary_data, SPLIT_STR):

        global solve
        # 判空
        if not require_name:
            return

        # 选择方法类并执行
        for mclass_instance in self.method_class_list:
            # 切分<class '###'> 切分得到的是一个main.###，是这个类名的全称
            mclass_fullname = str(mclass_instance.__class__).split('\'')[1]
            # 只要匹配出来了，就认为是找到了
            if re.findall(require_name + '$', mclass_fullname):
                # 区分接口类型，执行外层用户要求的方法
                solve = self.__distinct_interface(mclass_instance, binary_data, SPLIT_STR)
                break

        # 子方法执行成功后必须返回结果返回方法类执行方法的结果
        return solve

    # 区分接口类型，按照接口需要传入相应的方法
    def __distinct_interface(self, mclass_instance, binary_data, SPLIT_STR):
        global the_list
        global solve
        if isinstance(mclass_instance, IMethod):
            the_list = binary_data.decode('utf-8').split(SPLIT_STR)
        elif isinstance(mclass_instance, IBinaryFileMethod):
            the_list = binary_data.split(SPLIT_STR.encode('utf-8'))
        if the_list:
            solve = mclass_instance.invoke_algorithm(the_list)
        return solve

