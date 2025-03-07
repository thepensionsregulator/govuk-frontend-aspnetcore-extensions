# YouTube video 

In TPR pages, you can add a YouTube video block. ~~This is rendered using [ableplayer](https://ableplayer.github.io/ableplayer/).~~ 

## Using Tags Helpers (for ASP.NET Razor views)

```razor
@addTagHelper *, ThePensionsRegulator.Frontend
...
<tpr-youtube-video
    id="(UNIQUE_ID)" 
    title="(YOUTUBE_TITLE_FOR_ACCESSIBILITY)"
    videoId="(YOUTUBE_VIDEO_ID)" // https://www.youtube.com/watch?v=>>tTQiv1xKVM4<<
    useAblePlayer="false" // Redundant until AblePlayer is enabled
    preload="(OPTIONAL:true(default)|false)"
    autoplay="(OPTIONAL:true|false(default))"
    playsinline="metadata(default)|auto|none"
    transcriptUrl="(URL_TO_TRANSCRIPT)"
    transcriptTitle="(TRANSCRIPT_TITLE)"
    transcriptTarget="(OPTIONAL:_blank|_self|_parent|_top)"

    >
</tpr-youtube-video> 
```
## ~~Adding Ableplayer dependencies~~
>[!NOTE]
AblePlayer is not currently supported.

~~In order for your consuming web application to be able render the videos using Able Player, you will need to add the  dependencies to the page header. The easiest way to this is to render the `\Views\Shared\_VideoPlayerDependencies.cshtml` partial concluded with this package, inside the `head` tag of the page.~~

```razor
<head>
    ...
    <partial name="_VideoPlayerDependencies" />
</head>

```

## Umbraco block grid

Add a 'YouTube Video' component anywhere in a block grid using the  'TPR block list', 'TPR no forms block list', 'TPR block grid' or TPR accordion section block grid' data types. 


![Add a box component](/docs/images/youtube-video-block.png)

You have to fill in the following fields:
- Title - This is used for accessibility purposes. Is not visible to the end user.
- Url - This is the url of the YouTube video.
- Transcript url - (Optional) : This adds a link to the video transcript under the video.

In the settings tab, you can adjust the following properties:
- Autoplay (off by default) - Starts playing the video when the page loads. 
- Plays in line (on by default) - Instructs supporting browsers to play the video “inline” within the web page. 
- Preload - Tells the browser how much media to download when the page loads.

![Box settings](/docs/images/youtube-video-settings.png)

The video player will fill the width available to it. Please see the examples below of how it looks in different column widths.  

![Box examples in column layouts](/docs/images/youtube-video-example2.png)

