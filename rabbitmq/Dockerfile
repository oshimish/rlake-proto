FROM rabbitmq:3.11-management

COPY rabbitmq.conf /etc/rabbitmq/
COPY definitions.json /etc/rabbitmq/

# Copy the cookie file into the container
COPY .erlang.cookie /var/lib/rabbitmq/.erlang.cookie

# Make the cookie file readable by the rabbitmq user
RUN chown rabbitmq:rabbitmq /var/lib/rabbitmq/.erlang.cookie \
    && chmod 400 /var/lib/rabbitmq/.erlang.cookie