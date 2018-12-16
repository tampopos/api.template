FROM buildpack-deps:curl
COPY ./migration.sh /migration.sh
ENTRYPOINT [ "bash","migration.sh" ,"deploys-db/migration","60"]  