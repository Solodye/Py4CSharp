
from MethodServer.StrategyPatternModel import IBinaryFileMethod
from MethodServer import Server
import os


class FileHelper:

    @staticmethod
    def check_create_numdir(numdir_str):
        father_path_dir = "photodir\\"
        dir_path_str = father_path_dir + numdir_str
        if not os.path.exists(father_path_dir):
            os.mkdir(father_path_dir)
        if not os.path.exists(dir_path_str):
            os.mkdir(dir_path_str)
        return dir_path_str

    @staticmethod
    def save_picture(picture_name_str, byte_obj):
        # 保存前查询文件夹是否存在，不存在则在日志里记录异常
        dirstr = os.path.dirname(picture_name_str)
        if not os.path.exists(dirstr):
            raise FileExistsError("程序文件夹不存在，无法保存图片")
        if not os.path.isdir(dirstr):
            raise IsADirectoryError("图片路径有误")

        # 保存前查询同名历史文件是否存在，存在则尝试先删除
        if os.path.exists(picture_name_str) and os.path.isfile(picture_name_str):
            try:
                os.remove(picture_name_str)
            except:
                pass
        try:
            # 保存图片
            f = open(picture_name_str, 'wb')
            f.write(byte_obj)
            f.close()
        except:
            raise ValueError(picture_name_str + "二进制传入文件有问题")


class MatchNewRegPicture(IBinaryFileMethod):

    def invoke_algorithm(self, bynary_list):

        global returnmsg  # 这个是服务器匹配的结果

        # 方法类名， 工号字符串， id.jpg， picture.jpg
        if len(bynary_list) == 4:

            #   新建工号对应的文件夹
            '''
            father_path_dir = os.path.abspath(os.curdir) + "\\photodir\\"
            dir_path_str = father_path_dir + bynary_list[1].decode('utf-8')
            if not os.path.exists(father_path_dir):
                os.mkdir(father_path_dir)
            if not os.path.exists(dir_path_str):
                os.mkdir(dir_path_str)
            '''
            dir_path_str = FileHelper.check_create_numdir(bynary_list[1].decode('utf-8'))

            # 保存身份证图片
            '''
            idpic_path_str = dir_path_str + "\\id.jpg"
            f_idpic = open(idpic_path_str, 'wb')
            f_idpic.write(bynary_list[2])
            f_idpic.close()
            '''
            idpic_path_str = dir_path_str + "\\id.jpg"
            FileHelper.save_picture(idpic_path_str, bynary_list[2])

            # 保存摄像机图片
            '''
            phpic_path_str = dir_path_str + "\\picture.jpg"
            f_phpic = open(phpic_path_str, 'wb')
            f_phpic.write(bynary_list[3])
            f_phpic.close()
            '''
            phpic_path_str = dir_path_str + "\\person.jpg"
            FileHelper.save_picture(phpic_path_str, bynary_list[3])

            # 执行人脸识别算法
            returnmsg = self.__recog(idpic_path_str, phpic_path_str)

            # 删除遇到无法访问，在此抛异常则无法回传服务器计算结果
            # 考虑放弃删除，直接覆盖
            '''
            # #删除图片 弹出占用删不了
            # if os.path.exists(father_path_dir):
            #     os.remove(father_path_dir)
            # if os.path.exists(idpic_path_str):
            #     os.remove(idpic_path_str)
            '''
        else:
            returnmsg = "format_error"

        # 已经利用time.sleep()方法测试了多线程，效果不错
        '''
        # 测试多线程
        # print(bynary_list[1].decode('utf-8') + "sleep")
        # time.sleep(5)
        # print(bynary_list[1].decode('utf-8') + "aweak")
        '''

        return returnmsg

    def __recog(self, idpic_path_str, phpic_path_str):
        # 这里加上人脸识别的代码即可
        return ['True',"特征码"]


class MatchPicture(IBinaryFileMethod):
    def invoke_algorithm(self, bynary_list):

        global returnmsg  # 这个是服务器匹配的结果

        # 方法类名， 工号字符串，member.jpg
        if len(bynary_list) == 3:
            #  检查新建工号对应的文件夹
            dir_path_str = FileHelper.check_create_numdir(bynary_list[1].decode('utf-8'))

            # 保存现场拍摄图片
            mebpic_path_str = FileHelper.save_picture(dir_path_str + "\\member.jpg", bynary_list[2])

            # 自数据库查询并执行人脸识别算法
            returnmsg = self.__recog(mebpic_path_str)

        else:
            returnmsg = "format_error"
        return returnmsg

    def __recog(self, mebpic_path_str):
        # 这里加上人脸识别的代码即可

        return 'True'


Server.start(MatchNewRegPicture(), MatchPicture())
