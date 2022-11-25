FROM --platform=amd64 caddy:2.6.2-builder-alpine AS builder

RUN xcaddy build \
    --with github.com/mholt/caddy-webdav \
    --with github.com/greenpau/caddy-security

FROM --platform=amd64 caddy:2.6.2-alpine

COPY --from=builder /usr/bin/caddy /usr/bin/caddy
