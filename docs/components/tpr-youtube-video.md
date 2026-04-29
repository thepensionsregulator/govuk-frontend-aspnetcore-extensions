# YouTube video

The YouTube video component embeds a YouTube video in a page using the official YouTube player in privacy-enhanced mode.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend
...
<tpr-youtube-video
    id="example-video"
    title="Master Trusts conference by The Pensions Regulator"
    youtube-video-id="tTQiv1xKVM4"
    use-able-player="false"
    preload="auto"
    autoplay="false"
    plays-inline="true"
    description="<p>A summary of the video for users who cannot watch it.</p>"
    heading-level="h2"
    heading-size="govuk-heading-m"
    iframe-title="Master Trusts conference by The Pensions Regulator (video)"
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
| `description`       | `string` | Optional HTML description displayed between the heading and the video. Default is `null`.                                                                                                                                             |
| `heading-level`     | `string` | The HTML heading element used for the video title (e.g. `h2`, `h3`, `h4`, `h5`). Default is `h2`.                                                                                                                                     |
| `heading-size`      | `string` | The GOV.UK heading class applied to the heading (e.g. `govuk-heading-m`). Default is `govuk-heading-m`.                                                                                                                               |
| `iframe-title`      | `string` | The accessible title of the YouTube `<iframe>`, exposed to assistive technology. Defaults to the value of the `title` attribute.                                                                                                      |
| `plays-inline`      | `bool`   | Applies to iOS devices only. Sets whether to play the video within the web page rather than full-screen. Default is `true`.                                                                                                           |
| `preload`           | `string` | Applies when `use-able-player="true"`. Tells the browser how much media to download when the page loads. Valid values are `auto`, `metadata` or `none`. Defaults to `metadata`.                                                       |
| `title`             | `string` | A brief description of the video, displayed as the heading above the video.                                                                                                                                                           |
| `transcript-url`    | `string` | URL of a transcript of the video. Default is `null`.                                                                                                                                                                                  |
| `transcript-target` | `string` | Sets the `target` attribute of the link to the transcript when `transcript-url` is set. Default is `null`.                                                                                                                            |
| `transcript-title`  | `string` | Text used to link to a transcript when `transcript-url` is set. Default is `View transcript for '{{title}}'` where `{{title}}` is the value of the `title` attribute.                                                                 |
| `use-able-player`   | `bool`   | Use [Able Player](https://ableplayer.github.io/ableplayer/) instead of the official YouTube player. **Not currently supported. See [#381](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/issues/381)**. |
| `youtube-video-id`  | `string` | An alpha-numeric string uniquely identifying the video to embed, which can be found in the YouTube URL for the video.                                                                                                                 |

## Umbraco

Add a 'YouTube video' component anywhere in a block grid or block list using the 'TPR block list', 'TPR block grid' or 'TPR accordion section block grid' data types.

![Add a YouTube video component](/docs/images/youtube-video-block.png)

You can set the following properties:

- **Title** - A brief description of the video, displayed as the heading above the video.
- **URL** - The URL of the YouTube video which includes the YouTube video id.
- **Description** - Optional rich-text description displayed between the heading and the video.
- **Transcript URL** - Optional. A link to the transcript of the video.

When you provide a transcript URL you can set the link text in three ways:

- provide a title in the URL dialogue
- add a dictionary entry with the key 'YouTube video - transcript link text'
- leave both blank to use the default "View transcript for '{{title}}'"

In any of the above locations a `{{title}}` token is replaced with the title of the video.

The accessible iframe title is taken from the dictionary entry with the key 'Video title'. The default English value is `{{VIDEO_TITLE}} (video)`, where the `{{VIDEO_TITLE}}` token is replaced with the title of the video.

In the settings tab, you can set the following properties:

- **Autoplay** - Starts playing the video when the page loads. Default is off.
- **Plays inline** - Applies to iOS devices only. Sets whether to play the video within the web page rather than full-screen. Default is on.
- **Heading level** - The HTML heading element used for the video title. Choose from `Heading 2` to `Heading 5`. Defaults to `Heading 2` if left blank.
- **Heading class** - The GOV.UK heading class applied to the heading (e.g. `govuk-heading-m`). Defaults to `govuk-heading-m` if left blank.

![YouTube video settings](/docs/images/youtube-video-settings.png)

The video player will fill the width available to it. Please see the examples below of how it looks in different column widths.

![YouTube video examples](/docs/images/youtube-video-example2.png)

