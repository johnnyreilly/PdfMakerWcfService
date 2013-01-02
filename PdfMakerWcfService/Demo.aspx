<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="PdfMakerWcfService.Demo" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>PDF Maker demo</title>
    <style type="text/css">
        input, button {
            width: 400px;
        }
    </style>
</head>
<body>
    <h1>PDF Maker Demo</h1>

    <p>Enter a URL below and then click the relevant button to generate a PDF.</p>

    <form id="frmPdfMaker" action="PdfMaker.svc/GetPdf" method="get" target="_blank">    
        <div><input type="text" name="url" id="url" placeholder="http://your.url.com" /></div>
        <button type="submit">Generate PDF and display in new window</button>
    </form>

    <form id="frmAjaxPdfMaker" runat="server">
        <button type="button" id="generatePdfViaAjax">Generate PDF and display link below (AJAX)</button>
        <ul id="pdfUrls"></ul>
    </form>

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#generatePdfViaAjax").click(function () {

                var $ul = $("#pdfUrls"),
                    text = $("#url").val();

                $.getJSON("PdfMaker.svc/GetPdfUrl?" + $("#frmPdfMaker").serialize(), function (data) {

                    $ul.append('<li><a href="' + data + '" target="_blank">' + text + '</a></li>');
                });
            });
        });
    </script>
</body>
</html>
