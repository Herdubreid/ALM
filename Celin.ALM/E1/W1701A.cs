using System;

namespace Celin.W1701A
{
    public class Row
    {
        public string z_PRODM_362 { get; set; }
        public DateTime z_JCD_376 { get; set; }
        public string z_ACL8_372 { get; set; }
        public string z_DL01_581 { get; set; }
        public string z_DL01_583 { get; set; }
        public string z_ZE08_586 { get; set; }
        public string z_DL01_577 { get; set; }
        public string z_DL01_579 { get; set; }
        public string z_UITM_397 { get; set; }
        public string z_ACL6_370 { get; set; }
        public string z_ACL9_373 { get; set; }
        public string z_DL01_575 { get; set; }
        public string z_LATT_565 { get; set; }
        public string z_ZE09_588 { get; set; }
        public string z_ZE04_578 { get; set; }
        public string z_MMCU_481 { get; set; }
        public string z_ZE03_576 { get; set; }
        public string z_ACL4_368 { get; set; }
        public string z_DL01_591 { get; set; }
        public string z_DL01_550 { get; set; }
        public string z_MCU_522 { get; set; }
        public string z_DL01_589 { get; set; }
        public string z_DL01_547 { get; set; }
        public string z_DL01_585 { get; set; }
        public string z_RMK_529 { get; set; }
        public string z_DL01_587 { get; set; }
        public string z_APID_378 { get; set; }
        public string z_DL01_549 { get; set; }
        public string z_DL01_548 { get; set; }
        public string z_ACL5_369 { get; set; }
        public int z_AN8_359 { get; set; }
        public string z_DL02_524 { get; set; }
        public string z_WOYN_567 { get; set; }
        public string z_DL01_561 { get; set; }
        public string z_DL01_560 { get; set; }
        public string z_ACL2_366 { get; set; }
        public string z_DL01_358 { get; set; }
        public string z_DL01_556 { get; set; }
        public string z_DL01_555 { get; set; }
        public string z_DL01_558 { get; set; }
        public string z_DL01_557 { get; set; }
        public string z_DL01_552 { get; set; }
        public string z_DL01_551 { get; set; }
        public string z_DL01_554 { get; set; }
        public string z_RMK2_530 { get; set; }
        public string z_DL01_553 { get; set; }
        public string z_ZE02_574 { get; set; }
        public string z_DL03_525 { get; set; }
        public int z_AAID_568 { get; set; }
        public int z_LANO_360 { get; set; }
        public string z_DL01_559 { get; set; }
        public string z_LOC_410 { get; set; }
        public string z_ACL3_367 { get; set; }
        public string z_ZE05_580 { get; set; }
        public string z_PRODF_361 { get; set; }
        public string z_DL01_570 { get; set; }
        public string z_ZE01_569 { get; set; }
        public string z_LONG_564 { get; set; }
        public string z_ZE06_582 { get; set; }
        public string z_ACL0_374 { get; set; }
        public int z_NUMB_377 { get; set; }
        public string z_ZE07_584 { get; set; }
        public string z_DL01_562 { get; set; }
        public string z_ACL7_371 { get; set; }
        public string z_ZE10_590 { get; set; }
        public string z_EQST_364 { get; set; }
        public string z_ASID_357 { get; set; }
        public string z_CO_531 { get; set; }
        public string z_DL01_526 { get; set; }
        public string z_ACL1_365 { get; set; }
    }
    public class Response : AIS.FormResponse
    {
        public AIS.Form<AIS.FormData<Row>> fs_P1701_W1701A { get; set; }
    }
    public class Request : AIS.FormRequest
    {
        public Request()
        {
            outputType = AIS.Request.GRID_DATA;
            formName = "P1701_W1701A";
            returnControlIDs = "1";
            findOnEntry = "TRUE";
            maxPageSize = "10";
        }
    }
}
