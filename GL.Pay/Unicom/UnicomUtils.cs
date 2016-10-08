using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.Unicom
{
    public class UnicomUtils
    {

        //static function getRequestBean()
        //{
        //$bean = simplexml_load_string(file_get_contents('php://input'));
        //$request = array();
        //    foreach ($bean as $key => $value){
        //    $request[(string)$key] = (string) $value;
        //    }
        //    return $request;
        //}

        //static function toResponse($params, $rootName = 'xml')
        //{

        //    if (gettype($params) == 'array')
        //    {
        //    $entryXml = "";
        //        foreach ($params as $key => $value) {
        //        $entryXml = "${entryXml}<${key}>$value</${key}>";
        //        }
        //        return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><${rootName}>${entryXml}</${rootName}>";
        //    }
        //    if (!is_null($params))
        //    {
        //        return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><${rootName}>${params}</${rootName}>";
        //    }
        //    return '';
        //}

    }
}
