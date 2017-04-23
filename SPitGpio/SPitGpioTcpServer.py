import socket
import sys
import time
import RPi.GPIO as GPIO

from thread import *

GPIO.setmode(GPIO.BCM)
GPIO.setup(5, GPIO.OUT)
GPIO.setup(6, GPIO.OUT)
GPIO.setup(13, GPIO.OUT)
GPIO.setup(19, GPIO.OUT)
GPIO.setup(26, GPIO.OUT)
GPIO.setup(12, GPIO.OUT)
GPIO.setup(16, GPIO.OUT)
GPIO.setup(20, GPIO.OUT)
GPIO.setup(21, GPIO.OUT)
GPIO.setup(7, GPIO.OUT)
GPIO.setup(8, GPIO.OUT)

GPIO.setup(18, GPIO.IN)
GPIO.setup(23, GPIO.IN)
GPIO.setup(24, GPIO.IN)
GPIO.setup(25, GPIO.IN)
GPIO.setup(4, GPIO.IN)
GPIO.setup(17, GPIO.IN)
GPIO.setup(27, GPIO.IN)
GPIO.setup(22, GPIO.IN)

HOST = ''

SetGpioPort = 10000
GetGpioPprt = 10001

setGpioPinSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
getGpioPinSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

print 'Sockets created'

#Bind socket to local host and port
try:
    setGpioPinSocket.bind((HOST, SetGpioPort))
    getGpioPinSocket.bind((HOST, GetGpioPprt))
except socket.error as msg:
    print 'Bind failed. Error Code : ' + str(msg[0]) + ' Message ' + msg[1]
    sys.exit()
print 'Socket bind complete'

#Start listening on socket
setGpioPinSocket.listen(1)
getGpioPinSocket.listen(1)
print 'Socket now listening'

#Function for handling connections. This will be used to create threads
def processConnectionForSetGpio(conn):
    while True:
        data = conn.recv(1)
        if not data: break

        data = ord(data)

        state = (int('10000000', 2) & data) >> 7
        pin = (int('01111111', 2) & data)

        print 'processConnectionForSetGpio Pin: ' + str(pin) + " State: " + str(state)

        pinOutputState = GPIO.LOW
        if state > 0:
            pinOutputState = GPIO.HIGH

        GPIO.output(pin, pinOutputState)
    conn.close()

def processConnectionForGetGpio(conn):
    while True:
        data = conn.recv(1)
        if not data: break
        print 'processConnectionForGetGpio Data: ' + data
    conn.close()

def waitForSetGpioConnections():
    while 1:
        #wait to accept a connection - blocking call
        conn, addr = setGpioPinSocket.accept()
        print 'Connected with ' + addr[0] + ':' + str(addr[1])
        
        #start new thread takes 1st argument as a function name to be run, second is the tuple of arguments to the function.
        start_new_thread(processConnectionForSetGpio ,(conn,))

def waitForGetGpioConnections():
    while 1:
        #wait to accept a connection - blocking call
        conn, addr = getGpioPinSocket.accept()
        print 'Connected with ' + addr[0] + ':' + str(addr[1])
        
        #start new thread takes 1st argument as a function name to be run, second is the tuple of arguments to the function.
        start_new_thread(processConnectionForGetGpio ,(conn,))

start_new_thread(waitForSetGpioConnections ,())
start_new_thread(waitForGetGpioConnections ,())

raw_input("Press Enter to terminate.")