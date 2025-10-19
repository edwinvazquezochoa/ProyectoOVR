function DescargarExcel(fileName, fileBase64) {

    const link = document.createElement("a");
    link.download = fileName;
    /*link.href = "data:application/octet-stream;base64," + base64;*/
    link.href = "data:application/vnd.ms-excel;base64," + fileBase64;
    /* link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64Archivo;*/
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
