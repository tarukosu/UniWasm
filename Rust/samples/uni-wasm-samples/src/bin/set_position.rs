use uni_wasm::{Vector3, transform};

fn main() {}

#[no_mangle]
fn update() {
    let position = Vector3::new(1.0, 2.0, 3.0);

    transform::set_local_position(0, position);
}
