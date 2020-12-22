extern crate nalgebra as na;
extern crate num_traits;
use std::ops;

#[derive(Debug, Clone, Copy)]
pub struct Vector3 {
    pub x: f32,
    pub y: f32,
    pub z: f32,
}

impl Vector3 {
    pub fn new(x: f32, y: f32, z: f32) -> Vector3 {
        Vector3 { x, y, z }
    }
}

impl ops::Add for Vector3 {
    type Output = Vector3;
    fn add(self, v: Vector3) -> Vector3 {
        Vector3::new(self.x + v.x, self.y + v.y, self.z + v.z)
    }
}

impl ops::Mul<f32> for Vector3 {
    type Output = Vector3;
    fn mul(self, scale: f32) -> Self::Output {
        Vector3::new(self.x * scale, self.y * scale, self.z * scale)
    }
}

impl ops::Mul<Vector3> for f32 {
    type Output = Vector3;
    fn mul(self, vector: Vector3) -> Self::Output {
        Vector3::new(self * vector.x, self * vector.y, self * vector.z)
    }
}

pub mod time {
    pub fn get_time() -> f32 {
        unsafe {
            return time_get_time();
        }
    }

    pub fn get_delta_time() -> f32 {
        unsafe {
            return time_get_delta_time();
        }
    }

    extern "C" {
        fn time_get_time() -> f32;
        fn time_get_delta_time() -> f32;
    }
}

pub mod wasm_binding {
    #[repr(C)]
    pub struct Vector3 {
        pub x: f32,
        pub y: f32,
        pub z: f32,
    }

    #[repr(C)]
    pub struct Quaternion {
        pub x: f32,
        pub y: f32,
        pub z: f32,
        pub w: f32,
    }
}

pub mod common {
    pub use crate::wasm_binding;
    pub use crate::Vector3;

    pub type ElementIndex = i32;
    pub type ResourceIndex = i32;

    // pub type Vector3 = na::Vector3<f32>;
    //pub struct Vector3(na::Vector3<f32>);

    pub fn vector3_to_wasm_vector3(vector: Vector3) -> wasm_binding::Vector3 {
        wasm_binding::Vector3 {
            x: vector.x,
            y: vector.y,
            z: vector.z,
        }
    }
    /*
    impl Vector3 {
        pub(crate) fn to_wasm_binding(&self) -> wasm_binding::Vector3
        {
            wasm_binding::Vector3{ x:0.0, y:0.0, z:0.0}
        }
    }
    */
    /*
    #[repr(C)]
    pub struct Vector3 {
        pub x: f32,
        pub y: f32,
        pub z: f32,
    }
    */

    #[repr(C)]
    pub struct Quaternion {
        pub x: f32,
        pub y: f32,
        pub z: f32,
        pub w: f32,
    }
}

pub mod transform {
    pub use crate::common::ElementIndex;
    pub use crate::common::Quaternion;
    pub use crate::common::Vector3;

    pub use crate::common::vector3_to_wasm_vector3;
    pub use crate::wasm_binding;

    pub struct Transform {
        index: ElementIndex,
    }

    impl Transform {
        pub fn get_local_position(&self) -> Vector3 {
            get_local_position(self.index)
        }
        pub fn set_local_position(&self, position: Vector3) {
            set_local_position(self.index, position)
        }
        pub fn get_world_position(&self) -> Vector3 {
            get_world_position(self.index)
        }
        pub fn set_world_position(&self, position: Vector3) {
            set_world_position(self.index, position)
        }
        pub fn get_local_rotation(&self) -> Quaternion {
            get_local_rotation(self.index)
        }
        pub fn set_local_rotation(&self, rotation: Quaternion) {
            set_local_rotation(self.index, rotation)
        }
        pub fn get_world_rotation(&self) -> Quaternion {
            get_world_rotation(self.index)
        }
        pub fn set_world_rotation(&self, rotation: Quaternion) {
            set_world_rotation(self.index, rotation)
        }

        pub fn new(index: ElementIndex) -> Transform {
            Transform { index }
        }

        pub fn myself() -> Transform {
            Transform::new(0)
        }
    }

    pub fn get_local_position(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_local_position_x(object_id);
            let y = transform_get_local_position_y(object_id);
            let z = transform_get_local_position_z(object_id);
            Vector3::new(x, y, z)
            //Vector3 { x, y, z }
        }
    }

    pub fn set_local_position(object_id: ElementIndex, position: Vector3) {
        unsafe {
            let position = vector3_to_wasm_vector3(position);
            transform_set_local_position(object_id, position);
        }
    }

    pub fn get_world_position(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_world_position_x(object_id);
            let y = transform_get_world_position_y(object_id);
            let z = transform_get_world_position_z(object_id);
            Vector3::new(x, y, z)
            //Vector3 { x, y, z }
        }
    }

    pub fn set_world_position(object_id: ElementIndex, position: Vector3) {
        unsafe {
            transform_set_world_position(object_id, position);
        }
    }

    pub fn get_world_forward(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_world_forward_x(object_id);
            let y = transform_get_world_forward_y(object_id);
            let z = transform_get_world_forward_z(object_id);
            Vector3::new(x, y, z)
            //Vector3 { x, y, z }
        }
    }

    pub fn get_local_rotation(object_id: ElementIndex) -> Quaternion {
        unsafe {
            let x = transform_get_local_rotation_x(object_id);
            let y = transform_get_local_rotation_y(object_id);
            let z = transform_get_local_rotation_z(object_id);
            let w = transform_get_local_rotation_w(object_id);
            Quaternion { x, y, z, w }
        }
    }
    pub fn set_local_rotation(object_id: ElementIndex, rotation: Quaternion) {
        unsafe {
            transform_set_local_rotation(object_id, rotation);
        }
    }

    pub fn get_world_rotation(object_id: ElementIndex) -> Quaternion {
        unsafe {
            let x = transform_get_world_rotation_x(object_id);
            let y = transform_get_world_rotation_y(object_id);
            let z = transform_get_world_rotation_z(object_id);
            let w = transform_get_world_rotation_w(object_id);
            Quaternion { x, y, z, w }
        }
    }

    pub fn set_world_rotation(object_id: ElementIndex, rotation: Quaternion) {
        unsafe {
            transform_set_world_rotation(object_id, rotation);
        }
    }

    pub fn get_local_scale(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_local_scale_x(object_id);
            let y = transform_get_local_scale_y(object_id);
            let z = transform_get_local_scale_z(object_id);
            Vector3::new(x, y, z)
            //Vector3 { x, y, z }
        }
    }
    pub fn set_local_scale(object_id: ElementIndex, scale: Vector3) {
        unsafe {
            transform_set_local_scale(object_id, scale);
        }
    }
    pub fn get_world_scale(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_world_scale_x(object_id);
            let y = transform_get_world_scale_y(object_id);
            let z = transform_get_world_scale_z(object_id);
            Vector3::new(x, y, z)
            //Vector3 { x, y, z }
        }
    }
    pub fn set_world_scale(object_id: ElementIndex, scale: Vector3) {
        unsafe {
            transform_set_world_scale(object_id, scale);
        }
    }

    extern "C" {
        fn transform_get_local_position_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_position_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_position_z(object_id: ElementIndex) -> f32;
        fn transform_set_local_position(object_id: ElementIndex, position: wasm_binding::Vector3);

        fn transform_get_world_position_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_position_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_position_z(object_id: ElementIndex) -> f32;
        fn transform_set_world_position(object_id: ElementIndex, position: Vector3);

        fn transform_get_world_forward_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_forward_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_forward_z(object_id: ElementIndex) -> f32;

        fn transform_get_local_rotation_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_rotation_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_rotation_z(object_id: ElementIndex) -> f32;
        fn transform_get_local_rotation_w(object_id: ElementIndex) -> f32;
        fn transform_set_local_rotation(object_id: ElementIndex, rotation: Quaternion);

        fn transform_get_world_rotation_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_z(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_w(object_id: ElementIndex) -> f32;
        fn transform_set_world_rotation(object_id: ElementIndex, rotation: Quaternion);

        fn transform_get_local_scale_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_scale_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_scale_z(object_id: ElementIndex) -> f32;
        fn transform_set_local_scale(object_id: ElementIndex, scale: Vector3);

        fn transform_get_world_scale_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_scale_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_scale_z(object_id: ElementIndex) -> f32;
        fn transform_set_world_scale(object_id: ElementIndex, scale: Vector3);
    }
}

pub mod element {
    pub use crate::common::ElementIndex;
    pub use crate::common::{ResourceIndex, Vector3};
    pub use crate::transform;
    use transform::{Quaternion, Transform};

    pub struct Element {
        index: ElementIndex,
    }

    impl Element {
        /*
        pub fn get_local_position(&self) -> Vector3 {
            transform::get_local_position(self.index)
        }
        pub fn set_local_position(&self, position: Vector3) {
            transform::set_local_position(self.index, position)
        }
        pub fn get_world_position(&self) -> Vector3 {
            transform::get_world_position(self.index)
        }
        pub fn set_world_position(&self, position: Vector3) {
            transform::set_world_position(self.index, position)
        }
        pub fn get_local_rotation(&self) -> Quaternion {
            transform::get_local_rotation(self.index)
        }
        pub fn set_local_rotation(&self, rotation: Quaternion) {
            transform::set_local_rotation(self.index, rotation)
        }
        pub fn get_world_rotation(&self) -> Quaternion {
            transform::get_world_rotation(self.index)
        }
        pub fn set_world_rotation(&self, rotation: Quaternion) {
            transform::set_world_rotation(self.index, rotation)
        }
        */

        fn new(index: ElementIndex) -> Element {
            Element { index }
        }

        pub fn myself() -> Element {
            Element { index: 0 }
        }

        pub fn transform(&self) -> Transform {
            Transform::new(self.index)
        }
    }

    pub fn spawn_object_by_id(resource_id: &str) -> Result<Element, &str> {
        let resource_index = get_resource_index_by_id(resource_id);
        if resource_index < 0 {
            Err("Resource not found")
        } else {
            let element_index = spawn_object(resource_index);
            if element_index < 0 {
                Err("Spawn failed")
            } else {
                Ok(Element::new(element_index))
            }
        }
    }

    pub fn spawn_object(resource_index: ResourceIndex) -> ElementIndex {
        unsafe {
            return element_spawn_object(resource_index);
        }
    }

    pub fn get_resource_index_by_id(resource_id: &str) -> ResourceIndex {
        unsafe {
            let ptr = resource_id.as_ptr() as usize;
            let len = resource_id.len();
            return element_get_resource_index_by_id(ptr, len);
        }
    }

    extern "C" {
        fn element_spawn_object(resource_id: i32) -> ElementIndex;
        fn element_get_resource_index_by_id(ptr: usize, len: usize) -> ResourceIndex;
    }
}

pub mod physics {
    pub use crate::common::ElementIndex;
    pub use crate::common::Quaternion;
    pub use crate::common::Vector3;

    /*
    pub fn set_local_velocity(object_id: ElementIndex, velocity: Vector3)
    {
        unsafe
        {
            return physics_set_local_velocity(object_id, velocity);
        }
    }
    */

    pub fn set_world_velocity(object_id: ElementIndex, velocity: Vector3) {
        unsafe {
            return physics_set_world_velocity(object_id, velocity);
        }
    }

    extern "C" {
        // fn physics_set_local_velocity(object_id: ElementIndex, velocity: Vector3);
        fn physics_set_world_velocity(object_id: ElementIndex, velocity: Vector3);
    }
}
