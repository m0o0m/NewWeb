<?php
require_once './lib/SnsNetwork.php';
require_once './lib/SnsSigCheck.php';
require_once './lib/SnsStat.php';
require_once 'OpenApiV3.php';
$appid = 1103881749;
$appkey = 'thJ4XbABcGOhi5Kr';
$method = 'GET';
$url_path = '/taskback.php';
//print_r($_SERVER['PHP_SELF'] . $_SERVER["QUERY_STRING"]);
$secret = 'thJ4XbABcGOhi5Kr&';
$parr;
parse_str($_SERVER["QUERY_STRING"],$parr);

foreach ($parr as $key => $val ) 
{ 
	if($val==null){		
		$parr[$key]="";
	}
}

$isright = SnsSigCheck::verifySig( $method, $url_path, $parr, $secret,$_GET['sig']);
$socket;
$port = 5400;
$host = '115.159.3.167';
$potoType = 0;
//$filename="teststream".time().".txt";//要生成的图片名字
//$file = fopen("debug/".$filename,"w");//打开文件准备写入
//    fwrite($file,json_encode($parr));//写入
//    fclose($file);//关闭
if($isright == true){
    // if ($_GET['cmd']=='check')
    // {
		// echo '{"ret":0,"msg":"OK","zoneid":"1"}';
    // }else 
    // {
        
		if($_GET['step']==2){
			$potoType=5;
		}elseif($_GET['step']==3){
			$potoType=6;
		}else{
			exit('{"ret":0,"msg":"OK","zoneid":"1"}');
		}
    
		$socket = socket_create(AF_INET, SOCK_STREAM, SOL_TCP);
		//发送超时1秒
		socket_set_option($socket,SOL_SOCKET,SO_RCVTIMEO,array("sec"=>3, "usec"=>0 ) );
		//接收超时6秒
		socket_set_option($socket,SOL_SOCKET,SO_SNDTIMEO,array("sec"=>6, "usec"=>0 ) );
			if(!$socket){
				exit('创建socket失败');
			}
			$result = socket_connect($socket, $host, $port);
			if(!$result){
				exit('连接socket失败');
			}
			$result=socket_write($socket, pack('vva64c',69,$potoType,$_GET['openid'],0));
			if(!$result){
				exit('发送失败');
			}
		$out = socket_read($socket, 6);
		$arr = unpack("vv1/vv2/cc1/cc2",$out);
		if ($arr['c1']==0) {
			echo '{"ret":0,"msg":"OK","zoneid":"1"}';
		} else {
			echo '{"ret":3,"msg":"OK","zoneid":"1"}';
		}

		socket_close($socket); 
	// }
} else {
	echo '{"ret":3,"msg":"请求参数错误：（sig）","zoneid":"1"}';
}

 /* if ($isright==true) {
		print_r(json_encode(array(
				'ret' => '0',
				'msg' => 'OK',),true));
	} else {
		print_r(json_encode(array(
				'ret' => '4',
				'msg' => '请求参数错误：（sig）',),true));
	} */
?>