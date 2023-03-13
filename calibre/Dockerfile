FROM ubuntu
RUN apt-get update
ENV DEBIAN_FRONTEND=noninteractive
EXPOSE 6080
RUN apt-get install -y calibre
ENTRYPOINT '/usr/bin/calibre'

##docker run -it --net=host --env="DISPLAY" --volume="$HOME/.Xauthority:/root/.Xauthority:rw"