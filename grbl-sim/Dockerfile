FROM ubuntu as build
RUN apt-get update
RUN apt-get install -y git make gcc g++
RUN mkdir /app
WORKDIR /app
RUN git clone https://github.com/gnea/grbl.git .
RUN git checkout v1.1h.20190825
RUN git clone https://github.com/hrobeers/grbl-sim.git ./grbl/grbl-sim
WORKDIR /app/grbl/grbl-sim
RUN git checkout v1.1f-gcc-linux-fix
RUN make clean all

FROM ubuntu
RUN apt-get update
RUN apt-get install -y bash socat screen
RUN mkdir /app
WORKDIR /app
COPY --from=build /app/grbl/grbl-sim/grbl_sim.exe /app/grbl-sim
ENV GRBL_COMMENT="#\n"
ENV GRBL_REALTIME_FACTOR="1.0"
EXPOSE 9600
COPY start.sh /app/start.sh
ENTRYPOINT [ "/app/start.sh" ]


# FROM node:19.7
# RUN apt-get update
# RUN apt-get install -y bash git make gcc g++ socat screen gcc-avr linux-headers-generic
# RUN mkdir /app
# WORKDIR /app
# RUN git clone https://github.com/gnea/grbl.git .
# RUN git clone https://github.com/hrobeers/grbl-sim.git ./grbl/grbl-sim
# WORKDIR /app/grbl/grbl-sim
# RUN git checkout v1.1f-gcc-linux-fix
# RUN make new
# EXPOSE 9600
# COPY start.sh /app/start.sh
# ENTRYPOINT [ "/bin/bash" ]