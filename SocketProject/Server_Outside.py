from MethodServer.StrategyPatternModel import IMethod
from MethodServer import Server


class Plus(IMethod):

    def invoke_algorithm(self, str_list):
        if len(str_list) == 3:
            return int(str_list[1]) + int(str_list[2])
class PlusAndMinor(IMethod):

    def invoke_algorithm(self, str_list):
        if len(str_list) == 3:
            return [int(str_list[1]) + int(str_list[2]), int(str_list[1]) - int(str_list[2])]
        pass

class GetLongStr(IMethod):
    def invoke_algorithm(self, str_list):
            return "yiyifdwngwonariothahahahfewrfhnweotbewouirbweuob"

Server.start(Plus(), PlusAndMinor())