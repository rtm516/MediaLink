package com.rtm516.medialink;

import android.app.Notification;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.support.v4.media.session.MediaSessionCompat;
import android.util.Base64;

import androidx.core.app.NotificationCompat;
import androidx.core.app.NotificationManagerCompat;

public class NotificationHandler {
    public static final String CHANNEL_ID = "musiclink_chan1";

    public static final String ACTION_PREV = "action_prev";
    public static final String ACTION_PLAY = "action_play";
    public static final String ACTION_NEXT = "action_next";

    public static Notification notification;

    public static void createNotification(Context context, MediaInfoWrapper mediaWrapper) {
        NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.from(context);
        MediaSessionCompat mediaSessionCompat = new MediaSessionCompat(context, "musiclink_mediasession");

        byte[] decodedString = Base64.decode(mediaWrapper.data.thumbnail, Base64.DEFAULT);
        Bitmap icon = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);

        Intent intentPrev = new Intent(context, NotificationBroadcastReceiver.class).setAction(ACTION_PREV);
        PendingIntent pendingIntentPrev = PendingIntent.getBroadcast(context, 0, intentPrev, PendingIntent.FLAG_UPDATE_CURRENT);

        Intent intentPlay = new Intent(context, NotificationBroadcastReceiver.class).setAction(ACTION_PLAY);
        PendingIntent pendingIntentPlay = PendingIntent.getBroadcast(context, 0, intentPlay, PendingIntent.FLAG_UPDATE_CURRENT);

        Intent intentNext = new Intent(context, NotificationBroadcastReceiver.class).setAction(ACTION_NEXT);
        PendingIntent pendingIntentNext = PendingIntent.getBroadcast(context, 0, intentNext, PendingIntent.FLAG_UPDATE_CURRENT);

        notification = new NotificationCompat.Builder(context, CHANNEL_ID)
            .setSmallIcon(R.drawable.ic_music_note)
            .setContentTitle(mediaWrapper.data.title)
            .setContentText(mediaWrapper.data.artist)
            .setLargeIcon(icon)
            .setOnlyAlertOnce(true)
            .setShowWhen(false)
            .addAction(R.drawable.ic_skip_previous, "Previous", pendingIntentPrev)
            .addAction(mediaWrapper.data.playbackStatus.equals("Playing") ? R.drawable.ic_pause : R.drawable.ic_play_arrow, "Play", pendingIntentPlay)
            .addAction(R.drawable.ic_skip_next, "Next", pendingIntentNext)
            .setStyle(new androidx.media.app.NotificationCompat.MediaStyle()
                .setShowActionsInCompactView(0, 1, 2)
                .setMediaSession(mediaSessionCompat.getSessionToken()))
            .setPriority(NotificationCompat.PRIORITY_LOW)
            .build();

        notificationManagerCompat.notify(1, notification);
    }
}
