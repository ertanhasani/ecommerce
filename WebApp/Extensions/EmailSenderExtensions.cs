using System.Text.Encodings.Web;
using WebApp.Services;

namespace WebApp.Extensions;

public static class EmailSenderExtensions
{
    public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
    {
        #region content

        var content = $@"
<table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;'>
    <tbody>
        <tr style='border-collapse:collapse;'>
            <td align='center' style='padding:0;Margin:0;'>
                <table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;' width='600' cellspacing='0' cellpadding='0' bgcolor='#ffffff' align='center'>
                    <tbody>
                        <tr style='border-collapse:collapse;'>
                            <td align='left' style='Margin:0;padding-top:20px;padding-bottom:20px;padding-left:40px;padding-right:40px;'>
                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                    <tbody>
                                        <tr style='border-collapse:collapse;'>
                                            <td width='520' valign='top' align='center' style='padding:0;Margin:0;'>
                                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                    <tbody>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td class='es-m-txt-r' align='right' style='padding:0;Margin:0;'>
                                                                <h1 style='Margin:0;line-height:120%;mso-line-height-rule:exactly;font-size:30px;font-style:normal;font-weight:normal;color:#333333;font-family: Segoe UI !important;'>تایید ایمیل</h1>
                                                            </td>
                                                        </tr>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td align='left' style='padding:0;Margin:0;padding-top:5px;padding-bottom:20px;'>
                                                                <table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                                    <tbody>
                                                                        <tr style='border-collapse:collapse;'>
                                                                            <td style='padding:0;Margin:0px;border-bottom:1px solid #999999;background:none 0% 0% repeat scroll rgba(0, 0, 0, 0);height:1px;width:100%;margin:0px;'></td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        {(!string.IsNullOrEmpty(link) ? $@"
                                                        <tr style='border-collapse:collapse;'>
                                                            <td class='es-m-txt-c' align='center' style='padding:0;Margin:0;padding-top:20px;padding-bottom:20px;'>
                                                                <span class='es-button-border' style='border-style:solid;border-color:#00B8A9;background:#00B8A9;border-width:0px;display:inline-block;border-radius:0px;width:auto;'>
                                                                    <a href='{HtmlEncoder.Default.Encode(link)}' class='es-button' target='_blank' style='mso-style-priority:100 !important;text-decoration:none !important;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:18px;color:#FFFFFF;border-style:solid;border-color:#00B8A9;border-width:10px 20px;display:inline-block;background:#00B8A9;border-radius:0px;font-weight:normal;font-style:normal;line-height:120%;width:auto;text-align:center;font-family: Segoe UI !important;'>
                                                                        {(string.IsNullOrEmpty(link) ? "برو به" : link)}
                                                                    </a>
                                                                </span>
                                                            </td>
                                                        </tr>" : null)}
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
";

        #endregion

        #region getStyleTag

        var getStyleTag = @"
<style>
@media only screen and (max-width: 600px) {
    p, ul li, ol li, a {
        font-size: 16px !important
    }

    h1 {
        font-size: 30px !important;
        text-align: center
    }

    h2 {
        font-size: 26px !important;
        text-align: center
    }

    h3 {
        font-size: 20px !important;
        text-align: center
    }

    .es-menu td a {
        font-size: 16px !important
    }

    .es-header-body p,
    .es-header-body ul li,
    .es-header-body ol li,
    .es-header-body a {
        font-size: 16px !important
    }

    .es-footer-body p,
    .es-footer-body ul li,
    .es-footer-body ol li,
    .es-footer-body a {
        font-size: 16px !important
    }

    .es-infoblock p,
    .es-infoblock ul li,
    .es-infoblock ol li,
    .es-infoblock a {
        font-size: 12px !important
    }

    *[class='gmail-fix'] {
        display: none !important
    }

    .es-m-txt-c {
        text-align: center !important
    }

    .es-m-txt-r {
        text-align: right !important
    }

    .es-m-txt-l {
        text-align: left !important
    }

        .es-m-txt-r a img,
        .es-m-txt-c a img,
        .es-m-txt-l a img {
            display: inline !important
        }

    .es-button-border {
        display: block !important
    }

    .es-button {
        font-size: 20px !important;
        display: block !important;
        border-left-width: 0px !important;
        border-right-width: 0px !important
    }

    .es-btn-fw {
        border-width: 10px 0px !important;
        text-align: center !important
    }

    .es-adaptive table,
    .es-btn-fw,
    .es-btn-fw-brdr,
    .es-left,
    .es-right {
        width: 100% !important
    }

    .es-content table,
    .es-header table,
    .es-footer table,
    .es-content,
    .es-footer,
    .es-header {
        width: 100% !important;
        max-width: 600px !important
    }

    .es-adapt-td {
        display: block !important;
        width: 100% !important
    }

    .adapt-img {
        width: 100% !important;
        height: auto !important
    }

    .es-m-p0 {
        padding: 0px !important
    }

    .es-m-p0r {
        padding-right: 0px !important
    }

    .es-m-p0l {
        padding-left: 0px !important
    }

    .es-m-p0t {
        padding-top: 0px !important
    }

    .es-m-p0b {
        padding-bottom: 0 !important
    }

    .es-m-p20b {
        padding-bottom: 20px !important
    }

    .es-hidden {
        display: none !important
    }

    table.es-table-not-adapt,
    .esd-block-html table {
        width: auto !important
    }

    table.es-social td {
        display: inline-block !important
    }

    table.es-social {
        display: inline-block !important
    }
}
</style>
";

        #endregion

        #region head

        var head = @"<head>
                          <meta charset='UTF-8'>
                          <meta content='width=device-width, initial-scale=1' name='viewport'>
                          <meta content='telephone=no' name='format-detection'>
                          <title>New Template</title>
                          <!--[if (mso 16)]>
                            <style type='text/css'>
                            a {text-decoration: none;}
                            </style>
                            <![endif]-->
                          <!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]-->" + getStyleTag +
                   "</head>";

        #endregion

        #region GetHeader

        var getHeader = $@"<table class='es-header' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top;'>
    <tbody>
        <tr style='border-collapse:collapse;'>
            <td class='es-adaptive' align='center' style='padding:0;Margin:0;'>
                <table class='es-header-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#EFEFEF;' width='600' cellspacing='0' cellpadding='0' bgcolor='#efefef' align='center'>
                    <tbody>
                        <tr style='border-collapse:collapse;'>
                            <td align='left' style='Margin:0;padding-top:20px;padding-bottom:20px;padding-left:40px;padding-right:40px;'>
                                <table cellspacing='0' cellpadding='0' width='100%' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                    <tbody>
                                        <tr style='border-collapse:collapse;'>
                                            <td width='520' align='left' style='padding:0;Margin:0;'>
                                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                    <tbody>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td class='es-m-p0l' align='center' style='padding:0;Margin:0;'>
                                                                <a href='{link}' target='_blank' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#CCCCCC;'></a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>";

        #endregion

        #region getFooter

        var getFooter = $@"
<table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;'>
    <tbody>
        <tr style='border-collapse:collapse;'></tr>
        <tr style='border-collapse:collapse;'>
            <td align='center' style='padding:0;Margin:0;'>
                <table class='es-footer-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#EFEFEF;' width='600' cellspacing='0' cellpadding='0' bgcolor='#efefef' align='center'>
                    <tbody>
                        <tr style='border-collapse:collapse;'>
                            <td align='left' style='padding:20px;Margin:0;'>
                                <!--[if mso]><table width='560' cellpadding='0' cellspacing='0'><tr><td width='367' valign='top'><![endif]-->
                                <table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left;direction: ltr;'>
                                    <tbody>
                                        <tr style='border-collapse:collapse;'>
                                            <td class='es-m-p0r es-m-p20b' width='367' align='center' style='padding:0;Margin:0;'>
                                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                    <tbody>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td class='es-m-txt-с es-m-txt-l' esdev-links-color='#333333' align='left' style='padding:0;Margin:0;padding-bottom:10px;'>
                                                                <p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:20px;line-height:150%;color:#333333;'>
                                                                    Phone: 09129347829
                                                                    <br>
                                                                    Email: <a href='mailto:info@applon.com'>info@applon.com</a>
                                                                </p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <!--[if mso]></td><td width='20'></td><td width='173' valign='top'><![endif]-->
                                <table class='es-right' cellspacing='0' cellpadding='0' align='right' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right;'>
                                    <tbody>
                                        <tr style='border-collapse:collapse;'>
                                            <td class='es-m-p0r' width='173' align='center' style='padding:0;Margin:0;'>
                                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                    <tbody>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td class='es-m-p0l es-m-txt-c' align='center' style='padding:0;Margin:0;padding-bottom:10px;'>
                                                                <a href='{link}' target='_blank' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#333333;'>
                                                                </a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                        </tr>
                        <tr style='border-collapse:collapse;'>
                            <td align='left' style='padding:0;Margin:0;padding-left:20px;padding-right:20px;padding-bottom:30px;'>
                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                    <tbody>
                                        <tr style='border-collapse:collapse;'>
                                            <td width='560' valign='top' align='center' style='padding:0;Margin:0;'>
                                                <table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                    <tbody>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td align='center' class='es-m-txt-c' style='padding:0;Margin:0;'>
                                                                <p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;line-height:150%;color:#333333;font-family: Segoe UI !important;'>تولید کننده جزوه های دانشگاهی</p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style='border-collapse:collapse;'>
                                            <td align='center' style='padding:10px;Margin:0;'>
                                                <table class='es-table-not-adapt es-social' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>
                                                    <tbody>
                                                        <tr style='border-collapse:collapse;'>
                                                            <td valign='top' align='center' style='padding:0;Margin:0;padding-right:10px;'>
                                                                <a href='https://t.me/applon' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#333333;'>
                                                                    <img title='applon Telegram Account' src='{link}/images/social/telegram-logo-colored.png' alt='Tw' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;margin: 2px;background: white;'>
                                                                </a>
                                                            </td>
                                                            <td valign='top' align='center' style='padding:0;Margin:0;padding-right:10px;'>
                                                                <a href='https://twitter.com/intent/follow?original_referer={link}&amp;ref_src=twsrc%5Etfw&amp;region=follow_link&amp;screen_name=applon&amp;tw_p=followbutton' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#333333;'>
                                                                    <img title='applon Twitter Account' src='{link}/images/social/twitter-logo-colored.png' alt='Tw' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;margin: 2px;background: white;'>
                                                                </a>
                                                            </td>
                                                            <td valign='top' align='center' style='padding:0;Margin:0;padding-right:10px;'>
                                                                <a href='https://www.facebook.com/applon' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#333333;'>
                                                                    <img title='applon Facebook Account' src='{link}/images/social/facebook-logo-colored.png' alt='Fb' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;margin: 2px;background: white;'>
                                                                </a>
                                                            </td>
                                                            <td valign='top' align='center' style='padding:0;Margin:0;padding-right:10px;'>
                                                                <a href='https://www.instagram.com/applon' target='_blank' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#333333;'>
                                                                    <img title='applon Instagram Account' src='{link}/images/social/instagram-logo-colored.png' alt='Ig' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;margin: 2px;background: white;'>
                                                                </a>
                                                            </td>
                                                            <td valign='top' align='center' style='padding:0;Margin:0;padding-right:10px;'>
                                                                <a href='https://www.linkedin.com/company/applon' target='_blank' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;text-decoration:underline;color:#333333;'>
                                                                    <img title='applon Linkedin Account' src='{link}/images/social/linkedin-logo-colored.png' alt='In' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;margin: 2px;background: white;'>
                                                                </a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
";

        #endregion

        var message = $@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                      <html style='width:100%;direction:rtl !important;font-family: Segoe UI !important;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0;'>
                        {head}
                        <body style='width:100%;direction:rtl !important;font-family: Segoe UI !important;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0;'>
                            <div class='es-wrapper-color' style='background-color:#CCCCCC;'>
                              <!--[if gte mso 9]>
                                              <v:background xmlns:v='urn:schemas-microsoft-com:vml' fill='t'>
                                                  <v:fill type='tile' src='' color='#cccccc'></v:fill>
                                              </v:background>
                                          <![endif]-->
                                <table class='es-wrapper' width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;'>
                                    <tbody>
                                    <tr style='border-collapse:collapse;'>
                                        <td valign='top' style='padding-top: 30px;padding-bottom: 30px;Margin:0;'>
                                            {getHeader}
                                            {content}
                                            {getFooter}
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </body>                  
                </html>";

        return emailSender.SendEmailAsync(email, "تایید ایمیل", message);
    }


    #region SendResetPassword

    public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
    {
        return emailSender.SendEmailAsync(email, "بازنشانی رمز",
            $"برای بازنشانی رمز <a href='{callbackUrl}'>کلیک کنید</a>.");
    }

    #endregion
}