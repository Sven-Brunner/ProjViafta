<?php

require "connect.php";

class Api
{
    const PASSWORD = "GURKENSALAMIAUFLAUF";
    const FROM = 0;
    const TO = 9;
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

    public function getTops($from, $to, $username)
    {
        if (isset($username)) {
            $result = $this->db->query("SELECT * FROM score WHERE athlete = '%s' ORDER BY distance", $username);
        } else {
            if (!isset($from)) {
                $from = self::FROM;
            }
            if (!isset($to)) {
                $to = self::TO;
            }
            $result = $this->db->query("SELECT * FROM score ORDER BY distance LIMIT %s, %s", $from, $to);
        }
        $array = $result->fetch_all(MYSQLI_ASSOC);
        return $array;
    }
    public function post($distance, $maxacceleration, $athlete)
    {
        $this->db->query("INSERT INTO score (distance, max_acceleration, athlete, created) VALUES (%s, %s, '%s', NOW())", $distance, $maxacceleration, $athlete);
	}
}

?>