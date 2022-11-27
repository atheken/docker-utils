FROM alpine:3.17
ENV NO_UPDATE_NOTIFIER true
ENV npm_config_update_notifier false
RUN apk add npm python3 make
RUN apk --no-cache add g++ gcc libgcc libstdc++ linux-headers make python3 git && \
  ln -sf python3 /usr/bin/python && \
  python --version
RUN mkdir /app
WORKDIR /app
RUN npm install cncjs
EXPOSE 8000
ENV PATH="./node_modules/.bin/:${PATH}"
ENTRYPOINT [ "cncjs" ]