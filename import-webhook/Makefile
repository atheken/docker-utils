.PHONY: build

TAG ?= latest

build:
	dotnet test .
	dotnet publish --runtime=linux-musl-x64 -o ./build -c Release ./src/app/
	docker build -t atheken/$$(basename $$PWD):$(TAG) .
	docker push atheken/$$(basename $$PWD):$(TAG)