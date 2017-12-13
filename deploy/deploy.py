import os
import time


def deploy():
    os.system('sh deploy.sh')
    with open('deploy.txt', 'w+') as file:
        file.write(time.asctime() + '\n')

if __name__ == '__main__':
    deploy()
