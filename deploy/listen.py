import os
from http.server import BaseHTTPRequestHandler, HTTPServer
from deploy import deploy


class MyHandler(BaseHTTPRequestHandler):
    def do_HEAD(s):
        try:
            deploy()
        except:
            pass
        s.send_response(200)
        s.end_headers()

httpd = HTTPServer(('0.0.0.0', 2100), MyHandler)


try:
    httpd.serve_forever()
except KeyboardInterrupt:
    pass

httpd.server_close()

