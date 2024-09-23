#ifndef XENSIV_BGT60TRXX_CONF_H
#define XENSIV_BGT60TRXX_CONF_H

#define GESTURE

#ifdef GESTURE

//#define XENSIV_BGT60TRXX_CONF_DEVICE (XENSIV_DEVICE_BGT60TR13C)
//#define XENSIV_BGT60TRXX_CONF_START_FREQ_HZ (58000000000)
//#define XENSIV_BGT60TRXX_CONF_END_FREQ_HZ (63500000000)
//#define XENSIV_BGT60TRXX_CONF_NUM_SAMPLES_PER_CHIRP (64)
//#define XENSIV_BGT60TRXX_CONF_NUM_CHIRPS_PER_FRAME (64)
//#define XENSIV_BGT60TRXX_CONF_NUM_RX_ANTENNAS (2)
//#define XENSIV_BGT60TRXX_CONF_NUM_TX_ANTENNAS (1)
//#define XENSIV_BGT60TRXX_CONF_SAMPLE_RATE (2000000)
//#define XENSIV_BGT60TRXX_CONF_CHIRP_REPETITION_TIME_S (0.000591125)
//#define XENSIV_BGT60TRXX_CONF_FRAME_REPETITION_TIME_S (0.0772434)
//#define XENSIV_BGT60TRXX_CONF_NUM_REGS (38)
//
//#if defined(XENSIV_BGT60TRXX_CONF_IMPL)
//const uint32_t register_list[] = {
//    0x11e8270UL,
//    0x30a0210UL,
//    0x9e967fdUL,
//    0xb0805b4UL,
//    0xd102fffUL,
//    0xf010d00UL,
//    0x11000000UL,
//    0x13000000UL,
//    0x15000000UL,
//    0x17000be0UL,
//    0x19000000UL,
//    0x1b000000UL,
//    0x1d000000UL,
//    0x1f000b60UL,
//    0x21133c51UL,
//    0x235ff41fUL,
//    0x25701ce7UL,
//    0x2d000490UL,
//    0x3b000480UL,
//    0x49000480UL,
//    0x57000480UL,
//    0x5911be0eUL,
//    0x5b62fc0aUL,
//    0x5d03f000UL,
//    0x5f787e1eUL,
//    0x61a2a850UL,
//    0x63000c88UL,
//    0x65000172UL,
//    0x67000040UL,
//    0x69000000UL,
//    0x6b000000UL,
//    0x6d000000UL,
//    0x6f393b10UL,
//    0x7f000100UL,
//    0x8f000100UL,
//    0x9f000100UL,
//    0xad000000UL,
//    0xb7000000UL,
//};
//
//#endif /* XENSIV_BGT60TRXX_CONF_IMPL */

// Below was Okay
//#define XENSIV_BGT60TRXX_CONF_DEVICE (XENSIV_DEVICE_BGT60TR13C)
//#define XENSIV_BGT60TRXX_CONF_START_FREQ_HZ (58000000000)
//#define XENSIV_BGT60TRXX_CONF_END_FREQ_HZ (63500000000)
//#define XENSIV_BGT60TRXX_CONF_NUM_SAMPLES_PER_CHIRP (32)
//#define XENSIV_BGT60TRXX_CONF_NUM_CHIRPS_PER_FRAME (64)
//#define XENSIV_BGT60TRXX_CONF_NUM_RX_ANTENNAS (3)
//#define XENSIV_BGT60TRXX_CONF_NUM_TX_ANTENNAS (1)
//#define XENSIV_BGT60TRXX_CONF_SAMPLE_RATE (2000000)
//#define XENSIV_BGT60TRXX_CONF_CHIRP_REPETITION_TIME_S (0.0005911)
//#define XENSIV_BGT60TRXX_CONF_FRAME_REPETITION_TIME_S (0.0772258)
//#define XENSIV_BGT60TRXX_CONF_NUM_REGS (38)
//
//#if defined(XENSIV_BGT60TRXX_CONF_IMPL)
//const uint32_t register_list[] = {
//    0x11e8270UL,
//    0x30a0210UL,
//    0x9e967fdUL,
//    0xb0805b4UL,
//    0xd102bffUL,
//    0xf010d00UL,
//    0x11000000UL,
//    0x13000000UL,
//    0x15000000UL,
//    0x17000be0UL,
//    0x19000000UL,
//    0x1b000000UL,
//    0x1d000000UL,
//    0x1f000b60UL,
//    0x2113fc51UL,
//    0x237ff41fUL,
//    0x25701ce7UL,
//    0x2d000490UL,
//    0x3b000480UL,
//    0x49000480UL,
//    0x57000480UL,
//    0x5911be0eUL,
//    0x5b62fc0aUL,
//    0x5d03f000UL,
//    0x5f787e1eUL,
//    0x619c827cUL,
//    0x63001706UL,
//    0x650000d2UL,
//    0x67000020UL,
//    0x69000000UL,
//    0x6b000000UL,
//    0x6d000000UL,
//    0x6f2d0b10UL,
//    0x7f000100UL,
//    0x8f000100UL,
//    0x9f000100UL,
//    0xad000000UL,
//    0xb7000000UL,
//};
//#endif /* XENSIV_BGT60TRXX_CONF_IMPL */

// 30 Hz
#define XENSIV_BGT60TRXX_CONF_DEVICE (XENSIV_DEVICE_BGT60TR13C)
#define XENSIV_BGT60TRXX_CONF_START_FREQ_HZ (58000000000)
#define XENSIV_BGT60TRXX_CONF_END_FREQ_HZ (63500000000)
#define XENSIV_BGT60TRXX_CONF_NUM_SAMPLES_PER_CHIRP (32)
#define XENSIV_BGT60TRXX_CONF_NUM_CHIRPS_PER_FRAME (64)
#define XENSIV_BGT60TRXX_CONF_NUM_RX_ANTENNAS (3)
#define XENSIV_BGT60TRXX_CONF_NUM_TX_ANTENNAS (1)
#define XENSIV_BGT60TRXX_CONF_SAMPLE_RATE (2000000)
#define XENSIV_BGT60TRXX_CONF_CHIRP_REPETITION_TIME_S (0.0005079)
#define XENSIV_BGT60TRXX_CONF_FRAME_REPETITION_TIME_S (0.032512)
#define XENSIV_BGT60TRXX_CONF_NUM_REGS (38)

#if defined(XENSIV_BGT60TRXX_CONF_IMPL)
const uint32_t register_list[] = {
    0x11e8270UL,
    0x30a0210UL,
    0x9e967fdUL,
    0xb0805b4UL,
    0xd102bffUL,
    0xf010d00UL,
    0x11000000UL,
    0x13000000UL,
    0x15000000UL,
    0x17000be0UL,
    0x19000000UL,
    0x1b000000UL,
    0x1d000000UL,
    0x1f000b60UL,
    0x2113fc51UL,
    0x237ff41fUL,
    0x25701ce7UL,
    0x2d000490UL,
    0x3b000480UL,
    0x49000480UL,
    0x57000480UL,
    0x5911be0eUL,
    0x5b2c4a0aUL,
    0x5d03f000UL,
    0x5f787e1eUL,
    0x619c827cUL,
    0x63001706UL,
    0x650000d2UL,
    0x67000020UL,
    0x69000000UL,
    0x6b000000UL,
    0x6d000000UL,
    0x6f2c3b10UL,
    0x7f000100UL,
    0x8f000100UL,
    0x9f000100UL,
    0xad000000UL,
    0xb7000000UL,
};
#endif /* XENSIV_BGT60TRXX_CONF_IMPL */

#else

#define XENSIV_BGT60TRXX_CONF_DEVICE (XENSIV_DEVICE_BGT60TR13C)
#define XENSIV_BGT60TRXX_CONF_START_FREQ_HZ (61020000000)
#define XENSIV_BGT60TRXX_CONF_END_FREQ_HZ (61480000000)
#define XENSIV_BGT60TRXX_CONF_NUM_SAMPLES_PER_CHIRP (128)
#define XENSIV_BGT60TRXX_CONF_NUM_CHIRPS_PER_FRAME (64)
#define XENSIV_BGT60TRXX_CONF_NUM_RX_ANTENNAS (2)
#define XENSIV_BGT60TRXX_CONF_NUM_TX_ANTENNAS (1)
#define XENSIV_BGT60TRXX_CONF_SAMPLE_RATE (2352941)
#define XENSIV_BGT60TRXX_CONF_CHIRP_REPETITION_TIME_S (6.99625e-05)
#define XENSIV_BGT60TRXX_CONF_FRAME_REPETITION_TIME_S (0.100138)
#define XENSIV_BGT60TRXX_CONF_NUM_REGS (38)

#if defined(XENSIV_BGT60TRXX_CONF_IMPL)
const uint32_t register_list[] = {
    0x11e8270UL,
    0x3088210UL,
    0x9e967fdUL,
    0xb0805b4UL,
    0xd102fffUL,
    0xf010700UL,
    0x11000000UL,
    0x13000000UL,
    0x15000000UL,
    0x17000be0UL,
    0x19000000UL,
    0x1b000000UL,
    0x1d000000UL,
    0x1f000b60UL,
    0x21133c51UL,
    0x235ff41fUL,
    0x25006f7bUL,
    0x2d000490UL,
    0x3b000480UL,
    0x49000480UL,
    0x57000480UL,
    0x5911be0eUL,
    0x5b673c0aUL,
    0x5d03f000UL,
    0x5f787e1eUL,
    0x61f51fe8UL,
    0x630000a4UL,
    0x65000252UL,
    0x67000080UL,
    0x69000000UL,
    0x6b000000UL,
    0x6d000000UL,
    0x6f093910UL,
    0x7f000100UL,
    0x8f000100UL,
    0x9f000100UL,
    0xad000000UL,
    0xb7000000UL,
};

#endif /* XENSIV_BGT60TRXX_CONF_IMPL */

#endif

#endif /* XENSIV_BGT60TRXX_CONF_H */
