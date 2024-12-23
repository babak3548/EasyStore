

//var EMS_AppSetting_ImagePathInVirtual: string;
//var EMS_ImgDetailsSize: string;
//var EMS_ImgListSize: string;
//var EMS_ImgListSizeUI: string;


//export class helpTools { 

//    GetUrlDetailsImg(fileName: string | null | undefined) {
//        if (fileName === null || fileName === "" || fileName === "undefined" ) { return ""; }
//    else {
//        return EMS_AppSetting_ImagePathInVirtual.concat("/", fileName, EMS_ImgDetailsSize, "_", EMS_ImgDetailsSize + ".jpg");
//    }
//};
//}

//interface String {
//    GetUrlDetailsImg(): String;
//}

//String.prototype.GetUrlDetailsImg = function (): string {
//    if (String(this) === null || String(this) === "" || String(this) === "undefined") { return ""; }
//    else {
//        return EMS_AppSetting_ImagePathInVirtual.concat("/", String(this), EMS_ImgDetailsSize, "_", EMS_ImgDetailsSize + ".jpg");
//    }
//};



//declare global {
//    interface String {
//        GetUrlDetailsImg(msg: string): String;
//    }
//}
//String.prototype.GetUrlDetailsImg = function (msg: string): String {

//    return EMS_AppSetting_ImagePathInVirtual.concat("/", msg, EMS_ImgDetailsSize, "_", EMS_ImgDetailsSize);
//}
//export { }