FROM arm32v7/debian

RUN apt-get update && \
    apt-get install -y curl libunwind8 gettext apt-transport-https libicu52

RUN apt-get install -y build-essential automake libtool && \
	curl -sSL https://github.com/libuv/libuv/archive/v1.4.2.tar.gz | tar zxfv - -C /usr/local/src && \
	cd /usr/local/src/libuv-1.4.2 && \
	sh autogen.sh && \
	./configure && \
	make && \
	make install && \
	rm -rf /usr/local/src/libuv-1.4.2 && cd ~/ && \
	ldconfig

EXPOSE 5050
COPY . /app
WORKDIR /app

ENTRYPOINT ./GalconServer