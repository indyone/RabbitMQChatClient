## Chat Client over RabbitMQ

The concept for this exercise is to create a de-centralized chat service (meaning without a chat server to coordinate and manage the users or messages).
We will use the RabbitMQ message broker to publish and receive the messages that will follow a specific API.
Then we will implement a client that will use this API and provide the user interface for chatting with other users.

### The Chat Client Application

The chat client should have te following specifications:

* Must be implemented using .NET Core 3.0 (or 3.1!!!)
* Must be a console application
* It should use the Generic Host to configure and start the application.
* The RabbitMQ consumer should be implemented as a background service.

### The RabbitMQ Topology

The RabbitMQ topology for the char service is the following:

```
                 +------------------------------------------+
+----------+     |    +----------------+                    |
|          +-----+    |                |                    |
| Client 1 <----------+ Consumer Queue +<-----------------+ |
|          |          |                |                  | |
+----------+          +----------------+                  | |
                 +--------------------------+    +--------+-v------+
+----------+     |    +----------------+    |    |                 |
|          +-----+    |                |    +----> Fanout Exchange |
| Client 2 <----------+ Consumer Queue +<--------+                 |
|          |          |                |         |    chat_fnt     |
+----------+          +----------------+         |                 |
                                                 +--------+-^------+
+----------+          +----------------+                  | |
|          |          |                |                  | |
| Client N <----------+ Consumer Queue +<-----------------+ |
|          +-----+    |                |                    |
+----------+     |    +----------------+                    |
                 +------------------------------------------+
```

Where...

* **Client 1..N** - The chat client applications
* **Consumer Queue** - The queue that the client application will use to receive messages. This queue should be exclusive and auto-delete.
* **Fanout Exchange** - The exchange to publish messages. This exchange should be a fanout with the name `chat_fnt`.

### The Message API

The messages that the client should send and receive are in JSON format. Below are the sample messages:

#### Member Join

This message should be published by the chat client application when the member joins the chat service. 
When the other chat clients receive this message, they should display a notification text to the user.

```jsonc
{
    "type": "join", // The type of this message.
    "nickname": "indyone", // The name of the member that has joined the chat.
    "timestamp": 1575466048 // The Unix timestamp in seconds when the member has joined the chat.
}
```

#### Member Leave

This message should be published by the chat client application when the member leaves the chat service.
When the other chat clients receive this message, they should display a notification text to the user.

```jsonc
{
    "type": "leave", // The type of this message.
    "nickname": "indyone", // The name of the member that has left the chat.
    "timestamp": 1575466048 // The Unix timestamp in seconds when the member has left the chat.
}
```


#### Publish Message

This message should be published by the chat client application when the member sends some text.
When the other chat clients receive this message, they should display the published text to the user.

```jsonc
{
    "type": "publish", // The type of this message.
    "nickname": "indyone", // The name of the member that has published the text message.
    "timestamp": 1575466048, // The Unix timestamp in seconds when the member has published the text message.
    "message": "Hello World!"
}
```