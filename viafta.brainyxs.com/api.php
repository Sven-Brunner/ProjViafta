<?php

require "connect.php";

class Api
{
    const PASSWORD = "GURKENSALAMIAUFLAUF";
    private $db;

    public function __construct()
    {
        $this->db = new mysql();
    }

    public function validateRequest()
    {
        $signature = $_SERVER["HTTP_VERIFICATION_SIGNATURE"];
        $body = file_get_contents('php://input');
        $hmac = hash_hmac("sha256", $body, self::PASSWORD);
        return $hmac == $signature;
    }

    public function getTops($from = 0, $to = 9)
    {
        $result = $this->db->query("SELECT * FROM score ORDER BY distance LIMIT %s, %s", $from, $to);
        $array = $result->fetch_all(MYSQLI_ASSOC);
        return $array;
    }
}

?>