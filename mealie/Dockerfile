FROM --platform=amd64 hkotel/mealie:latest
RUN apt-get install redsocks
COPY ./start.sh /start
ENTRYPOINT [ "/start" ]