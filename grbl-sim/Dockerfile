FROM alpine:3.17
RUN apk update
RUN apk add bash git make gcc g++ socat
RUN mkdir /app
WORKDIR /app
RUN git clone https://github.com/gnea/grbl.git .
RUN git clone https://github.com/hrobeers/grbl-sim.git ./grbl/grbl-sim
WORKDIR /app/grbl/grbl-sim
RUN git checkout v1.1f-gcc-linux-fix
RUN make new
VOLUME /tmp
COPY start.sh /app/start.sh
ENTRYPOINT [ "/app/start.sh" ]