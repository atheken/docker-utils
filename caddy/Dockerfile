FROM caddy:2.6.2-builder-alpine AS builder

RUN xcaddy build \
    --with github.com/mholt/caddy-webdav \
    --with github.com/mholt/caddy-ratelimit \
    --with github.com/caddy-dns/duckdns

FROM caddy:2.6.2-alpine

COPY --from=builder /usr/bin/caddy /usr/bin/caddy
