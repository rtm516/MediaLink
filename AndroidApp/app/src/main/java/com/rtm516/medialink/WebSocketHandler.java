package com.rtm516.medialink;

import android.content.Context;
import android.os.Handler;
import android.os.PowerManager;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.java_websocket.client.WebSocketClient;
import org.java_websocket.handshake.ServerHandshake;

import java.net.URI;

public class WebSocketHandler extends WebSocketClient {
    private ObjectMapper objectMapper;
    private PowerManager.WakeLock wakeLock;
    private Context context;
    private boolean initialConnectionSuccess;

    public WebSocketHandler(Context context, URI serverUri) {
        super(serverUri);
        objectMapper = new ObjectMapper();
        this.context = context;
        initialConnectionSuccess = false;
    }

    @Override
    public void onOpen(ServerHandshake handshakedata) {
        PowerManager powerManager = (PowerManager) context.getSystemService(Context.POWER_SERVICE);
        wakeLock = powerManager.newWakeLock(PowerManager.PARTIAL_WAKE_LOCK, "MediaLink:WebSocket");
        wakeLock.acquire();

        System.out.println("Connected!");

        initialConnectionSuccess = true;
    }

    @Override
    public void onMessage(String message) {
        try {
            MediaInfoWrapper mediaWrapper = objectMapper.readValue(message, MediaInfoWrapper.class);
            if (mediaWrapper.hasData) {
                NotificationHandler.createNotification(MainActivity.getInstance(), mediaWrapper);
            }
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onClose(int code, String reason, boolean remote) {
        System.out.println("Closed " + code + " " + reason);
        if (initialConnectionSuccess) {
            // Reconnect on main thread
            Handler mainHandler = new Handler(context.getMainLooper());
            mainHandler.post(this::reconnect);
        } else {
            wakeLock.release();
        }
    }

    @Override
    public void onError(Exception ex) {
        ex.printStackTrace();
    }


}
