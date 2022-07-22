FROM node:12-alpine AS build
RUN apk update && apk upgrade && \
    apk add --no-cache git

RUN apk add python \
		python-dev \
		py-pip \
		build-base \
		&& pip install virtualenv \
		&& rm -rf /var/cache/apk/*

RUN npm config set unsafe-perm true
RUN npm install -g @angular/cli

# copying current project #
WORKDIR /UI
COPY /Control.Start.UI/ClientApp ./
RUN npm install
RUN ng build --prod=true --output-path=./dist --extract-css=true


FROM nginx:alpine AS runtime

COPY --from=build /UI/dist/ /usr/share/nginx/html
