#[cfg(test)]
mod tests {
    #[test]
    fn it_works() {
        assert_eq!(2 + 2, 4);
    }
}

pub mod time {
    pub fn get_time() -> f32
    {
        unsafe
        {
            return time_get_time();
        }
    }

    pub fn get_delta_time() -> f32
    {
        unsafe
        {
            return time_get_delta_time();
        }
    }

    extern "C" {
        fn time_get_time() -> f32;
        fn time_get_delta_time() -> f32;
    }
}

pub mod common
{
    pub type ElementIndex = i32;
    pub type ResourceIndex = i32;

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

pub mod transform {
    pub use crate::common::ElementIndex;
    pub use crate::common::Vector3;
    pub use crate::common::Quaternion;

    pub fn set_local_position(object_id: ElementIndex, position: Vector3)
    {
        unsafe
        {
            transform_set_local_position(object_id, position);
        }
    }

    pub fn set_world_position(object_id: ElementIndex, position: Vector3)
    {
        unsafe
        {
            transform_set_world_position(object_id, position);
        }
    }

    pub fn get_local_position(object_id: ElementIndex) -> Vector3
    {
        unsafe
        {
            let x = transform_get_local_position_x(object_id);
            let y = transform_get_local_position_y(object_id);
            let z = transform_get_local_position_z(object_id);
            Vector3 {
                x, y, z
            }
        }
    }

    pub fn get_world_position(object_id: ElementIndex) -> Vector3
    {
        unsafe
        {
            let x = transform_get_world_position_x(object_id);
            let y = transform_get_world_position_y(object_id);
            let z = transform_get_world_position_z(object_id);
            Vector3 {
                x, y, z
            }
        }
    }

    pub fn get_world_forward(object_id: ElementIndex) -> Vector3
    {
        unsafe
        {
            let x = transform_get_world_forward_x(object_id);
            let y = transform_get_world_forward_y(object_id);
            let z = transform_get_world_forward_z(object_id);
            Vector3 {
                x, y, z
            }
        }
    }

    pub fn set_local_rotation(object_id: ElementIndex, rotation: Quaternion)
    {
        unsafe
        {
            transform_set_local_rotation(object_id, rotation);
        }
    }

    pub fn get_world_rotation(object_id: ElementIndex) -> Quaternion
    {
        unsafe
        {
            let x = transform_get_world_rotation_x(object_id);
            let y = transform_get_world_rotation_y(object_id);
            let z = transform_get_world_rotation_z(object_id);
            let w = transform_get_world_rotation_w(object_id);
            Quaternion {
                x, y, z, w
            }
        }
    }

    pub fn set_world_rotation(object_id: ElementIndex, rotation: Quaternion)
    {
        unsafe
        {
            transform_set_world_rotation(object_id, rotation);
        }
    }

    /*
    pub fn get_local_rotation(object_id: ElementIndex) -> Quaternion
    {
        unsafe
        {
            return transform_get_local_rotation(object_id: ElementIndex);
        }
    }
    */

    pub fn set_local_scale(object_id: ElementIndex, scale: Vector3)
    {
        unsafe
        {
            transform_set_local_scale(object_id, scale);
        }
    }

    pub fn get_local_scale(object_id: ElementIndex) -> Vector3
    {
        unsafe
        {
            let x = transform_get_local_scale_x(object_id);
            let y = transform_get_local_scale_y(object_id);
            let z = transform_get_local_scale_z(object_id);
            Vector3 {
                x, y, z
            }
        }
    }

    extern "C" {
        fn transform_set_local_position(object_id: ElementIndex, position: Vector3);
        fn transform_get_local_position_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_position_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_position_z(object_id: ElementIndex) -> f32;

        fn transform_set_world_position(object_id: ElementIndex, position: Vector3);
        fn transform_get_world_position_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_position_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_position_z(object_id: ElementIndex) -> f32;

        fn transform_get_world_forward_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_forward_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_forward_z(object_id: ElementIndex) -> f32;

        fn transform_set_local_rotation(object_id: ElementIndex, rotation: Quaternion);

        fn transform_get_world_rotation_x(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_y(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_z(object_id: ElementIndex) -> f32;
        fn transform_get_world_rotation_w(object_id: ElementIndex) -> f32;

        fn transform_set_world_rotation(object_id: ElementIndex, rotation: Quaternion);
        // fn transform_get_local_rotation() -> Quaternion;
        fn transform_set_local_scale(object_id: ElementIndex, scale: Vector3);
        // fn transform_get_local_scale() -> Vector3;
        fn transform_get_local_scale_x(object_id: ElementIndex) -> f32;
        fn transform_get_local_scale_y(object_id: ElementIndex) -> f32;
        fn transform_get_local_scale_z(object_id: ElementIndex) -> f32;
    }
}


pub mod element {
    pub use crate::common::ElementIndex;
    pub use crate::common::ResourceIndex;
    
    pub fn spawn_object(resource_index: ResourceIndex) -> ElementIndex
    {
        unsafe
        {
            return element_spawn_object(resource_index);
        }
    }

    pub fn get_resource_index_by_id(id: &str) -> ResourceIndex
    {
        unsafe
        {
            return element_get_resource_index_by_id(id.as_ptr() as usize, id.len());
        }
    }

    extern "C" {
        fn element_spawn_object(resource_id: i32) -> ElementIndex;
        fn element_get_resource_index_by_id(ptr: usize, len: usize) -> ResourceIndex;
    }
}

pub mod physics {
    pub use crate::common::ElementIndex;
    pub use crate::common::Vector3;
    pub use crate::common::Quaternion;

    /*
    pub fn set_local_velocity(object_id: ElementIndex, velocity: Vector3)
    {
        unsafe
        {
            return physics_set_local_velocity(object_id, velocity);
        }
    }
    */

    pub fn set_world_velocity(object_id: ElementIndex, velocity: Vector3)
    {
        unsafe
        {
            return physics_set_world_velocity(object_id, velocity);
        }
    }

    extern "C" {
        // fn physics_set_local_velocity(object_id: ElementIndex, velocity: Vector3);
        fn physics_set_world_velocity(object_id: ElementIndex, velocity: Vector3);
    }
}
