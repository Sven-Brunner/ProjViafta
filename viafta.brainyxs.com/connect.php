<?php

class mysql {

    private $mysql = "";
    private $text = "";

    public function __construct(){
        global $config;
        $mysql_auth = array();
        $this->mysql = mysqli_init();

        $this->mysql->options(MYSQLI_OPT_CONNECT_TIMEOUT, 10);
        $this->mysql->options(MYSQLI_OPT_READ_TIMEOUT, 30);

        $mysql_auth['ip'] = "alosoguh.mysql.db.internal";
        $mysql_auth['username'] = "alosoguh_bot";
        $mysql_auth['password'] = "tbHgr7MhNBCjf?AWt67A";
        $mysql_auth['db'] = "alosoguh_viafta";

        try {
            $this->mysql->real_connect($mysql_auth['ip'], $mysql_auth['username'], $mysql_auth['password'], $mysql_auth['db']);
        }catch (Exception $e){
            if(stringContains($e->getMessage(), "Unknown database")){
                throw new SetupException("Given Database ".$config['mysql']['database']. " doesn't exist");
            }else{
                throw new MySQLException($e->getMessage(), $e->getCode(),$e->getPrevious());
            }
            echo "error";
            die();
        }

        $this->mysql->set_charset("utf8");
    }

    public function query(...$query){
        $build = "";
        foreach($query as $key => $value){
            if($key == "0"){
                $build = $value;
            } else {
                $build = $this->str_replace_first("%s", mysqli_real_escape_string($this->mysql, $value), $build);
            }
        }
        $this->text = $build;
        return $this->mysql->query($build);
    }

    public function getText(){
        return $this->text;
    }

    private function str_replace_first($from, $to, $content) {
        $from = '/'.preg_quote($from, '/').'/';
        return preg_replace($from, $to, $content, 1);
    }
}
$mysqli = new mysql();
