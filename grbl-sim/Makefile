.PHONY: build

TAG ?= latest

build:
	docker buildx create --use
	docker buildx build --platform=linux/amd64 --push -t atheken/$$(basename $$PWD):$(TAG) .
	docker buildx rm