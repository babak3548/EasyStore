"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.helpTools = void 0;
var EMS_AppSetting_ImagePathInVirtual;
var EMS_ImgDetailsSize;
var EMS_ImgListSize;
var EMS_ImgListSizeUI;
var helpTools = /** @class */ (function () {
    function helpTools() {
    }
    helpTools.prototype.GetUrlDetailsImg = function (fileName) {
        if (fileName === null || fileName === "" || fileName === "undefined") {
            return "";
        }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat("/", fileName, EMS_ImgDetailsSize, "_", EMS_ImgDetailsSize + ".jpg");
        }
    };
    ;
    return helpTools;
}());
exports.helpTools = helpTools;
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
//# sourceMappingURL=ExtentionMethods.js.map