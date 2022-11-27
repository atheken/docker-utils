FROM alpine:3.17
ENV NO_UPDATE_NOTIFIER true
ENV npm_config_update_notifier false
RUN apk --no-cache add bash npm eudev g++ gcc libgcc libstdc++ linux-headers make python3 git && \
  ln -sf python3 /usr/bin/python && \
  python --version
RUN mkdir /app
WORKDIR /app
RUN npm install cncjs
COPY start.sh /app/start
EXPOSE 8000
ENV PATH="/app/node_modules/.bin:${PATH}"
ENTRYPOINT [ "./start" ]
