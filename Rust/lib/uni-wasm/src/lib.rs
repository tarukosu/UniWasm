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
    pub type ObjectId = i32;

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
    pub use crate::common::ObjectId;
    pub use crate::common::Vector3;
    pub use crate::common::Quaternion;

    pub fn set_local_position(object_id: ObjectId, position: Vector3)
    {
        unsafe
        {
            transform_set_local_position(object_id, position);
        }
    }

    pub fn set_world_position(object_id: ObjectId, position: Vector3)
    {
        unsafe
        {
            transform_set_world_position(object_id, position);
        }
    }

    pub fn get_local_position(object_id: ObjectId) -> Vector3
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

    pub fn get_world_position(object_id: ObjectId) -> Vector3
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

    pub fn get_world_forward(object_id: ObjectId) -> Vector3
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

    pub fn set_local_rotation(object_id: ObjectId, rotation: Quaternion)
    {
        unsafe
        {
            transform_set_local_rotation(object_id, rotation);
        }
    }

    pub fn get_world_rotation(object_id: ObjectId) -> Quaternion
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

    pub fn set_world_rotation(object_id: ObjectId, rotation: Quaternion)
    {
        unsafe
        {
            transform_set_world_rotation(object_id, rotation);
        }
    }

    /*
    pub fn get_local_rotation(object_id: ObjectId) -> Quaternion
    {
        unsafe
        {
            return transform_get_local_rotation(object_id: ObjectId);
        }
    }
    */

    pub fn set_local_scale(object_id: ObjectId, scale: Vector3)
    {
        unsafe
        {
            transform_set_local_scale(object_id, scale);
        }
    }

    pub fn get_local_scale(object_id: ObjectId) -> Vector3
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
        fn transform_set_local_position(object_id: ObjectId, position: Vector3);
        fn transform_get_local_position_x(object_id: ObjectId) -> f32;
        fn transform_get_local_position_y(object_id: ObjectId) -> f32;
        fn transform_get_local_position_z(object_id: ObjectId) -> f32;

        fn transform_set_world_position(object_id: ObjectId, position: Vector3);
        fn transform_get_world_position_x(object_id: ObjectId) -> f32;
        fn transform_get_world_position_y(object_id: ObjectId) -> f32;
        fn transform_get_world_position_z(object_id: ObjectId) -> f32;

        fn transform_get_world_forward_x(object_id: ObjectId) -> f32;
        fn transform_get_world_forward_y(object_id: ObjectId) -> f32;
        fn transform_get_world_forward_z(object_id: ObjectId) -> f32;

        fn transform_set_local_rotation(object_id: ObjectId, rotation: Quaternion);

        fn transform_get_world_rotation_x(object_id: ObjectId) -> f32;
        fn transform_get_world_rotation_y(object_id: ObjectId) -> f32;
        fn transform_get_world_rotation_z(object_id: ObjectId) -> f32;
        fn transform_get_world_rotation_w(object_id: ObjectId) -> f32;

        fn transform_set_world_rotation(object_id: ObjectId, rotation: Quaternion);
        // fn transform_get_local_rotation() -> Quaternion;
        fn transform_set_local_scale(object_id: ObjectId, scale: Vector3);
        // fn transform_get_local_scale() -> Vector3;
        fn transform_get_local_scale_x(object_id: ObjectId) -> f32;
        fn transform_get_local_scale_y(object_id: ObjectId) -> f32;
        fn transform_get_local_scale_z(object_id: ObjectId) -> f32;
    }
}


pub mod object {
    pub use crate::common::ObjectId;
    pub fn spawn_object(resource_id: i32) -> ObjectId
    {
        unsafe
        {
            return object_spawn_object(resource_id);
        }
    }

    extern "C" {
        fn object_spawn_object(resource_id: i32) -> ObjectId;
    }
}

pub mod physics {
    pub use crate::common::ObjectId;
    pub use crate::common::Vector3;
    pub use crate::common::Quaternion;

    /*
    pub fn set_local_velocity(object_id: ObjectId, velocity: Vector3)
    {
        unsafe
        {
            return physics_set_local_velocity(object_id, velocity);
        }
    }
    */

    pub fn set_world_velocity(object_id: ObjectId, velocity: Vector3)
    {
        unsafe
        {
            return physics_set_world_velocity(object_id, velocity);
        }
    }

    extern "C" {
        // fn physics_set_local_velocity(object_id: ObjectId, velocity: Vector3);
        fn physics_set_world_velocity(object_id: ObjectId, velocity: Vector3);
    }
}
