package com.rtm516.medialink;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class NotificationBroadcastReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        switch (intent.getAction()) {
            case NotificationHandler.ACTION_PLAY:
                MainActivity.getInstance().getWebSocketHandler().safeSend("play");
                break;
            case NotificationHandler.ACTION_NEXT:
                MainActivity.getInstance().getWebSocketHandler().safeSend("next");
                break;
            case NotificationHandler.ACTION_PREV:
                MainActivity.getInstance().getWebSocketHandler().safeSend("prev");
                break;
        }
    }
}
