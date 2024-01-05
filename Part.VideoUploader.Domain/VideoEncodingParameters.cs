namespace Part.VideoUploader.Domain;

public class VideoEncodingParameters
{
    public string Format { get; set; } // e.g., "H.264", "H.265", "MJPEG", "MPEG-4"
    public string Resolution { get; set; } // e.g., "1080p", "720p", "480p"
    public int Bitrate { get; set; } // in kbps
    public double FrameRate { get; set; } // e.g., 30, 60
    public string AspectRatio { get; set; } // e.g., "16:9", "4:3"
    public bool EnableAudio { get; set; } // true or false
    public int AudioBitrate { get; set; } // in kbps, applicable if EnableAudio is true
    public int AudioSampleRate { get; set; } // e.g., 44100, 48000 Hz
    public string AudioChannels { get; set; } // e.g., "Stereo", "Mono"

}