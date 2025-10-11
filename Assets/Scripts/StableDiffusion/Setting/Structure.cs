using System;
using System.Collections;
using System.Collections.Generic;

public partial class SDsetting
{
    public static SDsetting setting = new SDsetting();

    public class SDProgress
    {
        public float progress;
        public float eta_relative;
        public ProgressState state;
        public string current_image;
        public string textinfo;
        
        [Serializable]
        public class ProgressState
        {
            public bool skipped;
            public bool interrupted;
            public bool stopping_generation;
            public string job;
            public int job_count;
            public string job_timestamp;
            public int job_no;
            public int sampling_step;
            public int sampling_steps;
        }
    }

    public class UpscalerModel
    {
        public string name;
        public string model_name;
        public string model_path;
        public string model_url;
        public float scale;
    }
    public class LatentUpscalerModel
    {
        public string name;
    }

    public class SDmodel
    {
        public string title; //.safetensors [e381203a3d] 붙는거
        public string model_name; //안붙고 그냥 이름만 붙는거
        public string hash;
        public string sha256;
        public string filename;
        public string config; //대부분 null
    }

    public class SD_Lora
    {
        public string path;

    }

    public class SDsampler
    {
        public string name;
        public string[] aliases;
        public SamplerOptions options;

        [Serializable]
        public class SamplerOptions
        {
            public string scheduler;
        }
    }

    public class SDscheduler
    {
        public string name;
        public string label;
        public string[] aliases;
        public float default_rho;
        public bool need_inner_model;
    }

    [Serializable]
    public class Config
    {
        public bool samples_save = true;
        public string samles_format;
        public bool save_images_add_number;
        public string save_images_replace_action;
        public bool grid_save;
        public string grid_format;
        public float jpeg_quality;
        public bool webp_lossless;
        public bool save_selected_only;
        public string outdir_samples;
        public string outdir_txt2img_samples;
        public string outdir_img2img_samples;
        public bool save_to_dirs;
        public string directories_filename_pattern;

        public string sd_model_checkpoint;
        public List<string> realesrgan_enabled_models;
        public string upscaler_for_img2img;
        public bool unload_sd_during_upscale;
        public float ESRGAN_tile;
        public float ESRGAN_tile_overlap;

        public bool face_restoration;
        public string face_restoration_model;
        public float code_former_weight;
        public bool face_restoration_unload;

        public bool api_enable_requests;
        public bool api_forbid_local_requests;
        public string auto_launch_browser;
        public bool show_warnings;
        public bool show_progress_in_title;
        public string localization;

        public bool pin_memory;
        public bool sd_checkpoints_keep_in_cpu;
        public bool cache_fp16_weight;
        public int sd_vae_checkpoint_cache;
        public bool enable_batch_seeds;

        public string sd_sampler_cfg_denoiser;
        public float s_min_uncond;
        public float sigma_min;
        public float sigma_max;
        public float rho;
    }


    public class RequestParams
    {
        public class Txt2ImageInBody
        {
            public bool enable_hr = false;
            public float denoising_strength = 0.4f;
            public int firstphase_width = 0;
            public int firstphase_height = 0;
            public string scheduler = "";
            public float hr_scale = 2;
            public float hr_cfg = 0;
            public string hr_upscaler = "";
            public int hr_second_pass_steps = 0;
            public int hr_resize_x = 0;
            public int hr_resize_y = 0;
            public string prompt = "";
            public string[] styles = { "" };
            public long seed = -1;
            public long subseed = -1;
            public float subseed_strength = 0;
            public int seed_resize_from_h = -1;
            public int seed_resize_from_w = -1;
            public string sampler_name = "Euler a";
            public int batch_size = 1;
            public int n_iter = 1;
            public int steps = 20;
            public float cfg_scale = 5;
            public int width = 1024;
            public int height = 1024;
            public bool restore_faces = false;
            public bool tiling = false;
            public string negative_prompt = "";
            public float eta = 0;
            public float s_churn = 0;
            public float s_tmax = 0;
            public float s_tmin = 0;
            public float s_noise = 1;
            public bool override_settings_restore_afterwards = true;
        }
    }
    public class ResponseParam
    {
        public class Txt2ImageOutBody
        {
            public string[] images;
            public SDParamsOutTxt2Img parameters;
            public string info;

            [Serializable]
            public class SDParamsOutTxt2Img
            {
                public bool enable_hr = false;
                public float denoising_strength = 0;
                public int firstphase_width = 0;
                public int firstphase_height = 0;
                public float hr_scale = 2;
                public string hr_upscaler = "";
                public int hr_second_pass_steps = 0;
                public int hr_resize_x = 0;
                public int hr_resize_y = 0;
                public string prompt = "";
                public string[] styles = { "" };
                public long seed = -1;
                public long subseed = -1;
                public float subseed_strength = 0;
                public int seed_resize_from_h = -1;
                public int seed_resize_from_w = -1;
                public string sampler_name = "Euler a";
                public int batch_size = 1;
                public int n_iter = 1;
                public int steps = 50;
                public float cfg_scale = 7;
                public int width = 512;
                public int height = 512;
                public bool restore_faces = false;
                public bool tiling = false;
                public string negative_prompt = "";
                public float eta = 0;
                public float s_churn = 0;
                public float s_tmax = 0;
                public float s_tmin = 0;
                public float s_noise = 1;
                public SettingsOveride override_settings;
                public bool override_settings_restore_afterwards = true;
                public string[] script_args = { };
                public string script_name = "";

                public class SettingsOveride
                {

                }
            }
        }
    }
}
