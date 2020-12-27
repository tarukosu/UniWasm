extern crate nalgebra as na;

use std::ops;

// Vector3
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

    pub fn forward() -> Vector3 {
        Vector3::new(1.0, 0.0, 0.0)
    }

    fn na_vec(self) -> na::Vector3<f32> {
        na::Vector3::new(self.x, self.y, self.z)
    }

    fn from_na_vec(v: na::Vector3<f32>) -> Vector3 {
        Vector3::new(v.x, v.y, v.z)
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

#[derive(Debug, Clone, Copy)]
pub struct Quaternion {
    pub x: f32,
    pub y: f32,
    pub z: f32,
    pub w: f32,
}

impl Quaternion {
    pub fn new(x: f32, y: f32, z: f32, w: f32) -> Quaternion {
        Quaternion { x, y, z, w }
    }

    pub fn forward(self) -> Vector3 {
        let q = self.na_quat();
        let v = q.transform_vector(&Vector3::forward().na_vec());
        Vector3::from_na_vec(v)
    }

    fn na_quat(self) -> na::UnitQuaternion<f32> {
        let q = na::Quaternion::new(self.w, self.x, self.y, self.z);
        na::UnitQuaternion::from_quaternion(q)
    }
}

mod utils {
    pub fn str_to_ptr(text: &str) -> (usize, usize) {
        let ptr = text.as_ptr() as usize;
        let len = text.len();
        return (ptr, len);
    }
}

pub mod debug {
    use crate::utils::str_to_ptr;

    pub fn log_info(message: &str) {
        unsafe {
            let (ptr, len) = str_to_ptr(message);
            debug_log_info(ptr, len);
        }
    }

    extern "C" {
        fn debug_log_info(ptr: usize, len: usize);
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
    pub use crate::{Quaternion, Vector3};

    pub type ElementIndex = i32;
    pub type ResourceIndex = i32;

    pub fn vector3_to_wasm_vector3(vector: Vector3) -> wasm_binding::Vector3 {
        wasm_binding::Vector3 {
            x: vector.x,
            y: vector.y,
            z: vector.z,
        }
    }

    pub fn quaternion_to_wasm_quaternion(rotation: Quaternion) -> wasm_binding::Quaternion {
        wasm_binding::Quaternion {
            x: rotation.x,
            y: rotation.y,
            z: rotation.z,
            w: rotation.w,
        }
    }
}

pub mod transform {
    pub use crate::common::ElementIndex;

    pub use crate::common::{quaternion_to_wasm_quaternion, vector3_to_wasm_vector3};
    pub use crate::{wasm_binding, Quaternion, Vector3};

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

        pub fn forward(self) -> Vector3 {
            let q = self.get_world_rotation();
            q.forward()
        }
    }

    pub fn get_local_position(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_local_position_x(object_id);
            let y = transform_get_local_position_y(object_id);
            let z = transform_get_local_position_z(object_id);
            Vector3::new(x, y, z)
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
        }
    }

    pub fn set_world_position(object_id: ElementIndex, position: Vector3) {
        unsafe {
            let position = vector3_to_wasm_vector3(position);
            transform_set_world_position(object_id, position);
        }
    }

    pub fn get_world_forward(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_world_forward_x(object_id);
            let y = transform_get_world_forward_y(object_id);
            let z = transform_get_world_forward_z(object_id);
            Vector3::new(x, y, z)
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
            let rotation = quaternion_to_wasm_quaternion(rotation);
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
            let rotation = quaternion_to_wasm_quaternion(rotation);
            transform_set_world_rotation(object_id, rotation);
        }
    }

    pub fn get_local_scale(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_local_scale_x(object_id);
            let y = transform_get_local_scale_y(object_id);
            let z = transform_get_local_scale_z(object_id);
            Vector3::new(x, y, z)
        }
    }
    pub fn set_local_scale(object_id: ElementIndex, scale: Vector3) {
        unsafe {
            let scale = vector3_to_wasm_vector3(scale);
            transform_set_local_scale(object_id, scale);
        }
    }
    pub fn get_world_scale(object_id: ElementIndex) -> Vector3 {
        unsafe {
            let x = transform_get_world_scale_x(object_id);
            let y = transform_get_world_scale_y(object_id);
            let z = transform_get_world_scale_z(object_id);
            Vector3::new(x, y, z)
        }
    }
    pub fn set_world_scale(object_id: ElementIndex, scale: Vector3) {
        unsafe {
            let scale = vector3_to_wasm_vector3(scale);
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
        fn transform_set_world_position(object_id: ElementIndex, position: wasm_binding::Vector3);

        fn transform_get_world_forward_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_forward_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_forward_z(object_id: ElementIndex) -> f32;

        fn transform_get_local_rotation_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_rotation_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_rotation_z(object_id: ElementIndex) -> f32;
        fn transform_get_local_rotation_w(object_id: ElementIndex) -> f32;
        fn transform_set_local_rotation(
            object_id: ElementIndex,
            rotation: wasm_binding::Quaternion,
        );

        fn transform_get_world_rotation_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_z(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_w(object_id: ElementIndex) -> f32;
        fn transform_set_world_rotation(
            object_id: ElementIndex,
            rotation: wasm_binding::Quaternion,
        );

        fn transform_get_local_scale_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_scale_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_scale_z(object_id: ElementIndex) -> f32;
        fn transform_set_local_scale(object_id: ElementIndex, scale: wasm_binding::Vector3);

        fn transform_get_world_scale_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_scale_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_scale_z(object_id: ElementIndex) -> f32;
        fn transform_set_world_scale(object_id: ElementIndex, scale: wasm_binding::Vector3);
    }
}

pub mod element {
    pub use crate::common::ElementIndex;
    pub use crate::common::{ResourceIndex, Vector3};
    pub use crate::{physics::Physics, transform};
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

        pub fn physics(&self) -> Physics {
            Physics::new(self.index)
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
    pub use crate::{
        common::{vector3_to_wasm_vector3, ElementIndex},
        wasm_binding, Vector3,
    };

    pub struct Physics {
        index: ElementIndex,
    }

    impl Physics {
        pub(crate) fn new(index: ElementIndex) -> Physics {
            Physics { index }
        }

        pub fn myself() -> Physics {
            Physics::new(0)
        }

        pub fn set_world_velocity(self, velocity: Vector3) {
            set_world_velocity(self.index, velocity)
        }
    }

    //pub use crate::common::Quaternion;
    //pub use crate::common::Vector3;

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
            let velocity = vector3_to_wasm_vector3(velocity);
            return physics_set_world_velocity(object_id, velocity);
        }
    }

    extern "C" {
        // fn physics_set_local_velocity(object_id: ElementIndex, velocity: Vector3);
        fn physics_set_world_velocity(object_id: ElementIndex, velocity: wasm_binding::Vector3);
    }
}
