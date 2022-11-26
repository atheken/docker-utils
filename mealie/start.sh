#!/usr/bin/env sh

# implement iptables socks routing and then call the base entrypoint.
echo "We'd configure redsocks before running mealie.."

exec /bin/sh -c $MEALIE_HOME/mealie/run.sh