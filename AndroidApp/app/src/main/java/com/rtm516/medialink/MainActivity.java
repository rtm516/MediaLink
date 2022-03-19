package com.rtm516.medialink;

import androidx.appcompat.app.AppCompatActivity;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import java.net.URI;

public class MainActivity extends AppCompatActivity {
    private static MainActivity instance;

    private boolean playing;
    private WebSocketHandler webSocketHandler;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        instance = this;

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        playing = false;

//        Button btnTest = findViewById(R.id.btnTest);

        NotificationChannel channel = new NotificationChannel(NotificationHandler.CHANNEL_ID, "Media display", NotificationManager.IMPORTANCE_LOW);
        NotificationManager notificationManager = getSystemService(NotificationManager.class);
        if (notificationManager != null) {
            notificationManager.createNotificationChannel(channel);
        }

        webSocketHandler = new WebSocketHandler(MainActivity.this, URI.create("ws://10.17.9.180:8181"));
        webSocketHandler.connect();

//        btnTest.setOnClickListener(v -> NotificationHandler.createNotification(MainActivity.this, playing));
    }

    public void togglePlaying() {
        playing = !playing;
//        NotificationHandler.createNotification(MainActivity.this, playing);
    }

    public WebSocketHandler getWebSocketHandler() {
        return webSocketHandler;
    }

    public static MainActivity getInstance() {
        return instance;
    }
}