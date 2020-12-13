use uni_wasm::transform;
use uni_wasm::object;
use uni_wasm::time;

fn main() {
}

#[no_mangle]
fn update() {
    // object::spawn_object(1);
}

#[no_mangle]
unsafe fn on_touch_start() {
    object::spawn_object(1);
}
