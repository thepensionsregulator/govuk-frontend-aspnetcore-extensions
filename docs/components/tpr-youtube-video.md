# YouTube video

The YouTube video component embeds a YouTube video in a page using the official YouTube player in privacy-enhanced mode.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend
...
<tpr-youtube-video
    video-id="example-video"
    title="Master Trusts conference by The Pensions Regulator"
    youtube-video-id="tTQiv1xKVM4"
    use-able-player="false"
    preload="auto"
    autoplay="false"
    plays-inline="true"
    transcript-url="https://example.org/my-video-transcript"
    transcript-title="View transcript of 'Master Trusts conference by The Pensions Regulator'"
    transcript-target="_self"
    />
```

## API

### `<tpr-youtube-video>`

| Attribute           | Type     | Description                                                                                                                                                                                                                           |
| ------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `autoplay`          | `bool`   | Start playing the video when the page loads. Should be avoided as it [creates accessibility problems and uses unnecessary bandwidth](https://abilitynet.org.uk/news-blogs/why-autoplay-accessibility-issue). Default is `false`.      |
| `plays-inline`      | `bool`   | Applies to iOS devices only. Sets whether to play the video within the web page rather than full-screen. Default is `true`.                                                                                                           |
| `preload`           | `string` | Applies when `use-able-player="true"`. Tells the browser how much media to download when the page loads. Valid values are `auto`, `metadata` or `none`. Defaults to `metadata`.                                                       |
| `title`             | `string` | A brief description of the video for assistive technology.                                                                                                                                                                            |
| `transcript-url`    | `string` | URL of a transcript of the video. Default is `null`.                                                                                                                                                                                  |
| `transcript-target` | `string` | Sets the `target` attribute of the link to the transcript when `transcript-url` is set. Default is `null`.                                                                                                                            |
| `transcript-title`  | `string` | Text used to link to a transcript when `transcript-url` is set. Default is `View transcript for '{{title}}'` where `{{title}}` is the value of the `title` attribute.                                                                 |
| `use-able-player`   | `bool`   | Use [Able Player](https://ableplayer.github.io/ableplayer/) instead of the official YouTube player. **Not currently supported. See [#381](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/issues/381)**. |
| `video-id`          | `string` | Sets the HTML id applied to the video. Takes precedence over `id` when the video is the outermost HTML element. Default is `null`.                                                                                                    |
| `youtube-video-id`  | `string` | An alpha-numeric string uniquely identifying the video to embed, which can be found in the YouTube URL for the video.                                                                                                                 |

## Umbraco

Add a 'YouTube video' component anywhere in a block grid or block list using the 'TPR block list', 'TPR block grid' or 'TPR accordion section block grid' data types.

![Add a YouTube video component](/docs/images/youtube-video-block.png)

You can set the following properties:

- **Title** - A brief description of the video for assistive technology.
- **URL** - The URL of the YouTube video which includes the YouTube video id.
- **Transcript URL** - Optional. A link to the transcript of the video.

When you provide a transcript URL you can set the link text in three ways:

- provide a title in the URL dialogue
- add a dictionary entry with the key 'YouTube video - transcript link text'
- leave both blank to use the default "View transcript for '{{title}}'"

In any of the above locations a `{{title}}` token is replaced with the title of the video.

In the settings tab, you can set the following properties:

- **Autoplay** - Starts playing the video when the page loads. Default is off.
- **Plays inline** - Applies to iOS devices only. Sets whether to play the video within the web page rather than full-screen. Default is on.
- **Preload** - Tells the browser how much media to download when the page loads.

![YouTube video settings](/docs/images/youtube-video-settings.png)

The video player will fill the width available to it. Please see the examples below of how it looks in different column widths.

![YouTube video examples](/docs/images/youtube-video-example2.png)

## ~~Adding Ableplayer dependencies~~

> [!NOTE] > [AblePlayer](https://ableplayer.github.io/ableplayer/) is not currently supported.

~~In order for your consuming web application to be able render the videos using Able Player, you will need to add the dependencies to the page header. The easiest way to this is to render the `\Views\Shared\_VideoPlayerDependencies.cshtml` partial concluded with this package, inside the `head` tag of the page.~~

```razor
<head>
    ...
    <partial name="_VideoPlayerDependencies" />
</head>

```
