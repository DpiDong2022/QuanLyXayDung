interface BaseResponse {
  message: string
  code: number
}

interface ApiResponse<T> extends BaseResponse {
  data?: T
}

export type { BaseResponse, ApiResponse }
